using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using Server.Game;

//PacketSession은 클라이언트에서 보낸 사람의 셰션을 말함
class PacketHandler
{
	public static void C_FirstEnterHandler(PacketSession session, IPacket packet)
	{
		Console.WriteLine("[Server] @>> RECV : C_FirstEnter");
		C_FirstEnter Rpkt = packet as C_FirstEnter;
		ClientSession clientSession = session as ClientSession;
		//Lock이 필요한가?

		Player player = null;
		player = PlayerManager.Instance.Add(clientSession.SessionId, Rpkt.playerNickName, false, clientSession) ;

		//입장과 동시에 로비로 넣어주기
		RoomManager.Instance.MoveIntroToLobbyRoom(player.Session.SessionId);

		Console.WriteLine($"클라로 부터 설정한 닉네임 받음 : { Rpkt.playerNickName}");
	}

	public static void C_SendChatHandler(PacketSession session, IPacket packet)
    {
		Console.WriteLine("[Server] @>> RECV : C_SendChat");
		C_SendChat Rpkt = packet as C_SendChat;
		LobbyRoom lobbyRoom = RoomManager.Instance.Find((int)Define.RoomID.Lobby);

		if (lobbyRoom == null)
			return;

		lobbyRoom.HandleChatting(Rpkt);
    }

	public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
	{
		Console.WriteLine("[Server] @>> RECV : C_LeaveGame");
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
