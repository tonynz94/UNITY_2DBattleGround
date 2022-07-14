using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using Server.Game;

class PacketHandler
{
	public static void C_FirstEnterHandler(PacketSession session, IPacket packet)
	{
		
		C_FirstEnter Rpkt = packet as C_FirstEnter;
		ClientSession clientSession = session as ClientSession;
		//Lock이 필요한가?

		Player player = null;
		player = PlayerManager.Instance.Add(clientSession.SessionId);
		player.Info.NickName = Rpkt.playerNickName;
		player.Info.isInGame = false;
		player.Session = clientSession;
		
		RoomManager.Instance.EnterToLobby(player);

		S_FirstEnter Spkt = new S_FirstEnter();
		Spkt.CGUID = player.Session.SessionId;
		Spkt.playerNickName = player.Info.NickName;
		clientSession.MyPlayer = player;

		clientSession.Send(Spkt.Write());

		Console.WriteLine($"클라로 부터 설정한 닉네임 받음 : { Rpkt.playerNickName}");
	}

	public static void C_SendChatHandler(PacketSession session, IPacket packet)
    {
		C_SendChat pkt = packet as C_SendChat;
		GameRoom gameRoom = RoomManager.Instance.Find((int)Define.RoomID.Lobby);
		gameRoom.Broadcast(pkt.Write());
    }

	public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
	{
		ClientSession clientSession = session as ClientSession;

		//if (clientSession.Room == null)
		//	return;

		//GameRoom room = clientSession.Room;
		//room.LeaveLobby(clientSession);
	}

	public static void C_MoveHandler(PacketSession session, IPacket packet)
	{
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;

		//if (clientSession.Room == null)
		//	return;

		////Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

		//GameRoom room = clientSession.Room;
		//room.Move(clientSession, movePacket);
	}
}
