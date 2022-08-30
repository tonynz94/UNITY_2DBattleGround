using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();

        //서버에 연결한 모든 플레이어들
        public Dictionary<int, Player> _players { get; private set; } = new Dictionary<int, Player>();

        object _lock = new object();
        
        public Player Add(int sesstionID, string nickName, bool isInGame, ClientSession clientSession)
        {
            lock (_lock)
            {
                Player player = new Player();
                player.Info.NickName = nickName;
                player.Session = clientSession;
                player.Session.MyPlayer = player;

                _players.Add(sesstionID, player);

                S_FirstEnter Spkt = new S_FirstEnter();
                Spkt.CGUID = player.Session.SessionId;
                Spkt.playerNickName = player.Info.NickName;

                BroadCast(Spkt.Write());

                return player;
            }
        }

        public void BroadCast(ArraySegment<byte> packet)
        {
            lock (_lock)
            {
                foreach (Player player in _players.Values)
                {
                    player.Session.Send(packet);
                }
            }
        }

        public Player GetPlayer(int CGUID)
        {
            Player player;
            _players.TryGetValue(CGUID, out player);
            return player;
        }

        public String GetPlayerNickName(int CGUID)
        {
            Player player = GetPlayer(CGUID);
            return player.Info.NickName;
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
            }
        }
    }
}
