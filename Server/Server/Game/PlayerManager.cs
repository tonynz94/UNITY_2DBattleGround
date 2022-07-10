using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();

        //서버에 연결한 모든 플레이어들
        Dictionary<int, Player> _players = new Dictionary<int, Player>();

        object _lock = new object();
        
        public Player Add(int sesstionID)
        {
            Player player = new Player();

            lock(_lock){
                _players.Add(sesstionID, player);
            }

            return player;

        }
    }
}
