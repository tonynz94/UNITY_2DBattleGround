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
                        sPkt.posX = rand.Next(-5, 5);
                        sPkt.posY = 0;
                        sPkt.posZ = rand.Next(-5, 5);
                        others.Session.Send(sPkt.Write());
                    }
                }
            }
        }

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
            Vector2Int cellPos = new Vector2Int(cPkt.CellPosX, cPkt.CellPosY);
            GameObject obj = FindObjectsInField(cellPos);
            if (obj != null)
                return;

            WaterBoomObject waterBoom = new WaterBoomObject(_roomID, cellPos);
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
                S_WaterBlowUp sPkt = new S_WaterBlowUp();
                sPkt.ID = waterBoomObject._id;
                Broadcast(sPkt.Write());

                _waterBoomObjectDic.Remove(waterBoomObject._id);
                
                CheckIsThereObjectsInBlowRange(waterBoomObject);
            }
         }

        public void CheckIsThereObjectsInBlowRange(WaterBoomObject waterBoomObject)
        {
            int blowXYRange = waterBoomObject.GetWaterBlowRange();

            for (int k = 1; k <= blowXYRange; k++)
            {
                Console.WriteLine($"({waterBoomObject.GetPos().x}, {waterBoomObject.GetPos().y}) +  ({ (Vector2Int.Up * k).x},{ (Vector2Int.Up * k).y})");
                Vector2Int cellPos = waterBoomObject.GetPos() + (Vector2Int.Up * k);

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
                    Console.WriteLine("player HIT!!");
                }
            }

            for (int k = 1; k <= blowXYRange; k++)
            {
                Vector2Int cellPos = waterBoomObject.GetPos() + (Vector2Int.Down * k);

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
                    Console.WriteLine("player HIT!!");
                }
            }

            for (int k = 1; k <= blowXYRange; k++)
            {
                Vector2Int cellPos = waterBoomObject.GetPos() + (Vector2Int.Left * k);

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
                    Console.WriteLine("player HIT!!");
                }
            }

            for (int k = 1; k <= blowXYRange; k++)
            {
                Vector2Int cellPos = waterBoomObject.GetPos() + (Vector2Int.Right * k);

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
                    Console.WriteLine("player HIT!!");
                }
            }
        }

        public void PlayerLeaveGame(int CGUID)
        {
            //lock (_lock)
            //{
            //    Player player = null;
            //    if (_.Remove(CGUID, out player) == false)
            //        return;

            //    player.Room = null;
            //    Map.ApplyLeave(player);

            //    // 본인한테 정보 전송
            //    {
            //        S_LeaveGame leavePacket = new S_LeaveGame();
            //        player.Session.Send(leavePacket);
            //    }
            //}





            //    // 타인한테 정보 전송
            //{
            //    S_Despawn despawnPacket = new S_Despawn();
            //    despawnPacket.ObjectIds.Add(objectId);
            //    foreach (Player p in _players.Values)
            //    {
            //        if (p.Id != objectId)
            //            p.Session.Send(despawnPacket);
            //    }
            //}
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

        public void Update()
        {
            //lock(_lock)
            //{
            //    foreach (GameField field in _playingGame.Values)
            //    {
            //        field.Update();
            //    }
            //}
        }

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
    }
}
