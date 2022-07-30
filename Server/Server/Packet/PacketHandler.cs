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
		RoomManager.Instance.MoveIntroToLobbyRoom(clientSession ,player.Session.SessionId);

		Console.WriteLine($"클라로 부터 설정한 닉네임 받음 : { Rpkt.playerNickName}");
	}
	
	public static void C_CreateGameRoomHandler(PacketSession session, IPacket packet)
	{
		Console.WriteLine("[Server] @>> RECV : C_CreateGameRoom");
		ClientSession clientSession = session as ClientSession;
		C_CreateGameRoom cPkt = packet as C_CreateGameRoom;

		RoomManager.Instance.HandleCreateGameRoom(cPkt);

	}

	public static void C_GetGameRoomsHandler(PacketSession session, IPacket packet)
	{
		Console.WriteLine("[Server] @>> RECV : C_GetGameRooms");
		ClientSession clientSession = session as ClientSession;

		RoomManager.Instance.HandleGetAllGameRooms(clientSession);
	}

	public static void C_IntroToLobbyHandler(PacketSession session, IPacket packet)
    {
		Console.WriteLine("[Server] @>> RECV : C_IntroToLobby");
		ClientSession clientSession = session as ClientSession;
		RoomManager.Instance.MoveIntroToLobbyRoom(clientSession, clientSession.SessionId);
    }
	
	//누군가 Join할때만 해당 패킷을 받음
	public static void C_LobbyToGameHandler(PacketSession session, IPacket packet)
	{
		Console.WriteLine("[Server] @>> RECV : C_LobbyToGame");
		C_LobbyToGame cPkt = packet as C_LobbyToGame;
		ClientSession clientSession = session as ClientSession;

		RoomManager.Instance.MoveLobbytToGameRoom(clientSession.SessionId, cPkt.roomId);

	}

	public static void C_ClickReadyOnOffHandler(PacketSession session, IPacket packet)
    {
		Console.WriteLine("[Server] @>> RECV : C_ClickReadyOnOff");
		C_ClickReadyOnOff cPkt = packet as C_ClickReadyOnOff;

		RoomManager.Instance.HandleReadyInGameRoom(cPkt);
	}

	public static void C_GameToLobbyHandler(PacketSession session, IPacket packet)
	{
		Console.WriteLine("[Server] @>> RECV : C_GameToLobby");
		C_GameToLobby cPkt = packet as C_GameToLobby;
		ClientSession clientSession = session as ClientSession;
		RoomManager.Instance.MoveGameToLobbyRoom(clientSession.SessionId, cPkt.roomId);
	}

	public static void C_SendChatHandler(PacketSession session, IPacket packet)
    {
		Console.WriteLine("[Server] @>> RECV : C_SendChat");
		C_SendChat cPkt = packet as C_SendChat;
		LobbyRoom lobbyRoom = RoomManager.Instance.GetLobby();

		if (lobbyRoom == null)
			return;

		lobbyRoom.HandleChatting(cPkt);
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
