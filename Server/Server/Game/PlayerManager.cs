using System;
using System.Collections.Generic;
using System.Text;

// Repository (사용자 데이터)
// Service / Manager



namespace Server.Game
{
    class PlayerFactory
    {
        Player CreateNewlyLoggedInPlayer() {
                Player player = new Player();
                player.Info.NickName = nickName;
                player.Session = clientSession;
                player.Session.MyPlayer = player;
                return player
        }
    }

    class PlayerRepository
    {
        //서버에 연결한 모든 플레이어들
        private readonly Dictionary<int, Player> _playersBySessionId { get; private set; } = new Dictionary<int, Player>();

        public void Add(Player player) 
        {
            lock();
        }
        public Player GetBySessionId(int CGUID)
        {
            Player player;
            _players.TryGetValue(CGUID, out player);
            return player;
        }

        public String GetNickName(int CGUID)
        {
            Player player = GetPlayer(CGUID);
            return player.Info.NickName;
        }

        public ICollection<Player> FindPlayers()
        {
            return players.Values;
        }
    }

    class Broadcaster 
    {
        public void SubscribeUser(Player player) 
        {
            player.LoginEvent += OnPlayerLogin;
        }

        void OnPlayerLogin(Player player) {
            
            S_FirstEnter event = new S_FirstEnter();
            event.CGUID = player.Session.SessionId;
            event.playerNickName = player.Info.NickName;

            BroadcastToAllPlayers(event);
        }

        public void BroadcastToAllPlayers(ArraySegment<byte> packet)
        {
            IList<Player> allPlayers = playerRepository.FindAll();
            lock(_lock) 
            {
                allPlayers.ForEach(player => player.Session.Send(packet);)
            }
        }

    }


    class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();


        object _lock = new object();
        
        public Player AddNewlyLoggedInPlayer(string nickName, ClientSession clientSession)
        {

            lock (_lock)
            {
                Player player = gameState.Find(nickName);
                if (player != null) {
                    // 못들어가게...
                }

                Player player = CreateNewlyLoggedInPlayer(session, nickName);
                playerRepository.Add(player);

                broadcaster.SubscribeUser(player);

                player.FirstEnterEvent.Invoke();
            }
        }

        private static Player CreateNewlyLoggedInPlayer(ClientSession session, string nickName) {
                Player player = new Player()
                {
                    SessionId = clientSession.Id,
                    Info.NickName = nickName;
                }
                
                return player;
        }

       

        public void HandlePlayerSkillPoint(C_SkillState cPkt)
        {
            lock(_lock)
            {
                Player player = GetPlayer(cPkt.CGUID);

                if (player == null)
                    Console.WriteLine("There are no Player ");

                player.Info.SpeedUpPoint = cPkt.SpeedUpPoint;
                player.Info.RangeUpPoint = cPkt.RangeUpPoint;
                player.Info.PowerUpPoint = cPkt.PowerUpPoint;
                player.Info.WaterCountUpPoint = cPkt.WaterCountUpPoint;

                //이동속도는 클라에서 컨트롤해줌
                S_SkillState sPkt = new S_SkillState();
                sPkt.Speed = Data.DataManager.SpeedUpDict[cPkt.SpeedUpPoint].speed;
                GetPlayer(cPkt.CGUID).Session.Send(sPkt.Write());
                
            }
        }
    }
}
