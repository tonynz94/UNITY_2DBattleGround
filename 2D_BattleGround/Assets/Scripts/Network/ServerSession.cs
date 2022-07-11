﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;
using UnityEngine;

namespace DummyClient
{
	class ServerSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Debug.Log($"OnConnected : {endPoint}");

			C_FirstEnter firstEnter = new C_FirstEnter();
			firstEnter.playerNickName = Managers.Player.NickName;
			Managers.Net.Send(firstEnter.Write());
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Debug.Log($"OnDisconnected : {endPoint}");
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer, (s, p) => PacketQueue.Instance.Push(p));
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
