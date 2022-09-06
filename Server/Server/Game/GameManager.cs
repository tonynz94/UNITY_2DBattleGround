using System;
using System.Collections.Generic;
using System.Text;
using Server.Util;
using static Server.Define;

namespace Server.Game
{
    class GameField
    {
        object _lock = new object();

        int _roomID;
        int _waterBoomID = 333;
        MapType _mapID;
        GameMode _gameMode;
        Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        Dictionary<int, GameObject> _waterBoomObjectDic = new Dictionary<int, GameObject>();
        LinkedList<GameObject> _itemObejcttList = new LinkedList<GameObject>();

        public GameField(int roomID, MapType mapID, GameMode gameMode)
        {
            _roomID = roomID;
            _mapID = mapID;
            _gameMode = gameMode;
        }

        public int GetPlayerCount()
        {
            return _playerDic.Count;
        }

        public void EnterToField(int CGUID)
        {
            lock (_lock)
            {
                Player player = PlayerManager.Instance.GetPlayer(CGUID);
                if (player == null)
                {
                    Console.WriteLine("Theres no player in Game plz check sink");
                    return;
                }

                GameRoom gameRoom = RoomManager.Instance.GetGameRoom(_roomID);

                if (gameRoom == null && gameRoom._isStarted == false)
                {
                    Console.WriteLine("Theres no GameRoom or Its not Started yet");
                    return;
                }

                _playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));

                Random rand = new Random();
                //본인에게 전송
                {
                    foreach (var others in gameRoom._playerList)
                    {
                        //ClientSession session = SessionManager.Instance.Find(CGUID);
                        S_EnterFieldWorld sPkt = new S_EnterFieldWorld();
                        sPkt.CGUID = CGUID;
                        sPkt.slotIndex = _playerDic.Count - 1;
                        others.Session.Send(sPkt.Write());
                    }
                }
            }
        }

        //절대값으로 보내 줌
        public void PlayerMove(C_Move cPkt)
        {
            // 좌표 바꿔주고
            Player player = PlayerManager.Instance.GetPlayer(cPkt.CGUID);
            player.Info.cellPosX = cPkt.cellPosX;
            player.Info.cellPosY = cPkt.cellPosY;
            player.Info.Dir = (Define.MoveDir)cPkt.Dir;
            player.Info.State = (Define.ObjectState)cPkt.State;

            // 모두에게 알린다
            S_BroadcastMove sPkt = new S_BroadcastMove();
            sPkt.CGUID = player.Session.SessionId;
            sPkt.posX = cPkt.posX;
            sPkt.posY = cPkt.posY;
            sPkt.cellPosX = (int)player.Info.cellPosX;
            sPkt.cellPosY = (int)player.Info.cellPosY;
            sPkt.Dir = (int)player.Info.Dir;
            sPkt.State = (int)player.Info.State;
            Broadcast(sPkt.Write());
        }

        public Player FindPlayersInField(Vector2Int cellPos)
        {
            foreach (Player player in _playerDic.Values)
            {
                if (new Vector2Int(player.Info.cellPosX, player.Info.cellPosY) == new Vector2Int(cellPos.x, cellPos.y))
                    return player;
            }

            return null;
        }

        public GameObject FindObjectsInField(Vector2Int cellPos)
        {
            GameObject objectInField = null;

            foreach (WaterBoomObject obj in _waterBoomObjectDic.Values)
            {
                WaterBoomObject boom = obj;

                if (boom._cellPos == cellPos)
                    return obj;
            }

            return null;
        }

        public void SetWaterBoomInField(C_WaterBOOM cPkt)
        {
            Player player = _playerDic[cPkt.CGUID];
            if (!player.isPossableWaterBoomSettedInField())
                return;

            Vector2Int cellPos = new Vector2Int(cPkt.CellPosX, cPkt.CellPosY);
            GameObject obj = FindObjectsInField(cellPos);
            if (obj != null)
                return;

            player.SetWaterBoomCountUp();

            int range = Data.DataManager.RangeUpDict[player.Info.RangeUpPoint].range;
            float damge = 100 * Data.DataManager.PowerUpDict[player.Info.PowerUpPoint].power;
            WaterBoomObject waterBoom = new WaterBoomObject(cPkt.CGUID, _roomID, cellPos, (int)damge, range);
            waterBoom._ownerID = cPkt.CGUID;
            waterBoom._roomID = cPkt.roomID;
            waterBoom._id = _waterBoomID++;
            _waterBoomObjectDic.Add(waterBoom._id, waterBoom);

            S_WaterBOOM sPkt = new S_WaterBOOM();
            sPkt.ID = waterBoom._id;
            sPkt.CellPosX = cPkt.CellPosX;
            sPkt.CellPosY = cPkt.CellPosY;

            Broadcast(sPkt.Write());
        }

        public void BlowWaterBoom(WaterBoomObject waterBoomObject)
        {
            lock (_lock)
            {
                Player player = _playerDic[waterBoomObject._ownerID];
                player.SetWaterBoomCountDown();

                S_WaterBlowUp sPkt = new S_WaterBlowUp();
                sPkt.ID = waterBoomObject._id;
                Broadcast(sPkt.Write());

                _waterBoomObjectDic.Remove(waterBoomObject._id);
                
                CheckIsThereObjectsInBlowRange(waterBoomObject, Vector2Int.Up);
                CheckIsThereObjectsInBlowRange(waterBoomObject, Vector2Int.Down);
                CheckIsThereObjectsInBlowRange(waterBoomObject, Vector2Int.Left);
                CheckIsThereObjectsInBlowRange(waterBoomObject, Vector2Int.Right);
            }
         }

        public void CheckIsThereObjectsInBlowRange(WaterBoomObject waterBoomObject, Vector2Int dir)
        {
            int blowXYRange = waterBoomObject.GetWaterBlowRange();

            for (int k = 1; k <= blowXYRange; k++)
            {
                Console.WriteLine($"({waterBoomObject.GetPos().x}, {waterBoomObject.GetPos().y}) +  ({ (dir * k).x},{ (dir * k).y})");
                Vector2Int cellPos = waterBoomObject.GetPos() + (dir * k);

                GameObject obj = FindObjectsInField(cellPos);
                Player player = FindPlayersInField(cellPos);
                if (obj != null)
                {
                    if (obj._objectType == ObjectType.WaterBoom)
                        (obj as WaterBoomObject).WaterBoomBlowUp();

                    break;
                }
                else if (player != null)
                {
                    int hitPlayerLeftHP = Math.Max(player.Info.currentHP - waterBoomObject.GetDamage(), 0);

                    player.Info.currentHP = hitPlayerLeftHP;

                    S_PlayerHit sPkt1 = new S_PlayerHit();
                    sPkt1.Damage = waterBoomObject.GetDamage();
                    sPkt1.HitPlayerLeftHP = player.Info.currentHP;
                    sPkt1.CGUID = player.Session.SessionId;
                    Broadcast(sPkt1.Write());

                    if( hitPlayerLeftHP <= 0)
                    {
                        S_PlayerDie sPkt2 = new S_PlayerDie();
                        sPkt2.CGUID = player.Session.SessionId;
                        sPkt2.AttackerCGUID = waterBoomObject._ownerID;
                        sPkt2.Damage = waterBoomObject.GetDamage();
                        Broadcast(sPkt2.Write());
                    }
                }
            }
        }

        public void PlayerLeaveGame(int CGUID)
        {
            Player player = null;
            _playerDic.TryGetValue(CGUID, out player);
            _playerDic.Remove(CGUID);

            player.LeaveFromGameRoomOrField();

            RoomManager.Instance.GetLobby().EnterLobbyRoom(CGUID);
        }
      
        public void Update()
        {
            lock(_lock)
            {
                foreach(WaterBoomObject waterBoom in _waterBoomObjectDic.Values)
                {
                    waterBoom.Update();
                }
            }
        }
            
        public void Broadcast(ArraySegment<byte> packet)
        {
            lock (_lock)
            {
                foreach (Player player in _playerDic.Values)
                {
                    player.Session.Send(packet);
                }
            }
        }
    }

    class GameManager
    {
        object _lock = new object();

        public static GameManager Instance { get; } = new GameManager();
       
        //실행되고 있는 게임들 목록
        public Dictionary<int, GameField> _playingGame = new Dictionary<int, GameField>();

        public void NewGameStart(int roomID)
        {
            MapType mapType = RoomManager.Instance.GetGameRoom(roomID)._mapType;
            GameMode gameMode = RoomManager.Instance.GetGameRoom(roomID)._gameMode;
            _playingGame.Add(roomID, new GameField(roomID ,mapType, gameMode));
        }
        
        public GameField GetGameField(int roomID)
        {
            GameField gameField = null;
            _playingGame.TryGetValue(roomID, out gameField);

            return gameField;
        }

        public void HandlePlayerEnterToField(int roomID,int CGUID)
        {
            lock (_lock)
            {
                GameField gameField = null;
                _playingGame.TryGetValue(roomID, out gameField);

                if (gameField == null)
                {
                    Console.WriteLine("Theres is no GameID Started plz Check");
                }
                else
                {
                    gameField.EnterToField(CGUID);
                }
            }
        }

        public void HandleMove(C_Move cPkt)
        {
            GameField gameField = GetGameField(cPkt.roomID);

            if (gameField == null)
            {
                Console.WriteLine("Not Exist GameRoom or on Field");
                return;
            }

            gameField.PlayerMove(cPkt);
        }

        public void HandleWaterBOOM(C_WaterBOOM cPkt)
        {
            GameField gameField = GetGameField(cPkt.roomID);
            if (gameField == null)
            {
                Console.WriteLine("There are no GameField plz check");
                return;
            }
            
            gameField.SetWaterBoomInField(cPkt);
        }

        public void MoveGameFieldToLobby(C_FieldToLobby cPkt)
        {
            GameField gameField = GetGameField(cPkt.RoomID);
            gameField.PlayerLeaveGame(cPkt.CGUID);

            if (gameField.GetPlayerCount() == 0)
                GameFinish(cPkt.RoomID);
        }

        public void GameFinish(int roomID)
        {
            lock (_lock)
            {
                GameField gameField = GetGameField(roomID);
                _playingGame.Remove(roomID);
                RoomManager.Instance.RemoveGameRoom(roomID);
            }
        }
    }
}
