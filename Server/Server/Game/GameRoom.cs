using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	//게임 중인 방 정보
	class GameRoom
	{
		//0번은 로비 
		//1 ~ n번은 실행중인 게임
		public int RoomID { get; set; }
		Dictionary<int, Player> _players = new Dictionary<int, Player>();
		List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

		public void Flush()
		{
			foreach (KeyValuePair<int, Player> player in _players)
				player.Value.Session.Send(_pendingList);
			//Console.WriteLine($"Flushed {_pendingList.Count} items");
			_pendingList.Clear();
		}

		public void Broadcast(ArraySegment<byte> segment)
		{
			_pendingList.Add(segment);			
		}

		public void EnterLobby(Player player)
		{
			//로비에 추가
			_players.Add(player.Session.SessionId, player);
			player.Room = this;

			//// 신입생한테 모든 플레이어 목록 전송
			//S_PlayerList players = new S_PlayerList();
			//foreach (ClientSession s in _sessions)
			//{
			//	players.players.Add(new S_PlayerList.Player()
			//	{
			//		isSelf = (s == session),
			//		playerId = s.SessionId,
			//		posX = s.PosX,
			//		posY = s.PosY,
			//		posZ = s.PosZ,
			//	});
			//}
			//session.Send(players.Write());

			//// 신입생 입장을 모두에게 알린다
			//S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
			//enter.playerId = session.SessionId;
			//enter.posX = 0;
			//enter.posY = 0;
			//enter.posZ = 0;
			//Broadcast(enter.Write());
		}

		public void LeaveLobby(ClientSession session)
		{
			// 플레이어 제거하고
			_players.Remove(session.SessionId);

			// 모두에게 알린다
			//S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
			//leave.playerId = session.SessionId;
			//Broadcast(leave.Write());
		}

		public void Move(ClientSession session, C_Move packet)
		{
			// 좌표 바꿔주고
			session.PosX = packet.posX;
			session.PosY = packet.posY;
			session.PosZ = packet.posZ;

			// 모두에게 알린다
			S_BroadcastMove move = new S_BroadcastMove();
			move.playerId = session.SessionId;
			move.posX = session.PosX;
			move.posY = session.PosY;
			move.posZ = session.PosZ;
			//Broadcast(move.Write());
		}
	}
}
