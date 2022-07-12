using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Server.Game;

namespace Server
{
	//연결 수 만큼의 인스턴스가 만들어 져 있음
	class ClientSession : PacketSession
	{
		public Player MyPlayer;
		public int SessionId { get; set; }
		public float PosX { get; set; }
		public float PosY { get; set; }
		public float PosZ { get; set; }

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}, SessionID : {SessionId}");

			S_HandShake sPkt = new S_HandShake();
			Send(sPkt.Write());

		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			SessionManager.Instance.Remove(this);
			//if (Room != null)
			//{
			//	GameRoom room = Room;
			//	room.LeaveLobby(this);
			//	Room = null;
			//}

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
