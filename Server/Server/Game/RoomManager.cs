using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    //로비 방
    //게임중인인 방 (중간중간 추가될수 있음)
    class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();

        //로비룸
        GameRoom _Lobby = new GameRoom();

        //실행중인 게임룸
        Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        object _lock = new object();
        int roomId = 1;

        //실행 중인 게임 추가
        public void EnterToLobby(Player player)
        {
            _Lobby.EnterLobby(player);
        }

        public GameRoom Add()
        {
            GameRoom gameRoom = new GameRoom();
            lock(_lock)
            {
                gameRoom.RoomID = roomId++;
                _rooms.Add(gameRoom.RoomID, gameRoom);
            }
            return gameRoom;
        }

        //끝난 게임 삭제
        public GameRoom Remove(int roomId)
        {
            return null;
        }

        public GameRoom Find(int roomId)
        {
            if(roomId == 0)
            {
                return _Lobby;
            }
            else
            {
                GameRoom gameRoom;
                _rooms.TryGetValue(roomId, out gameRoom);
                if (gameRoom != null)
                    return gameRoom;
            }

            Console.WriteLine($"theres no such a room {roomId}");
            return null;
        }
    }
}
