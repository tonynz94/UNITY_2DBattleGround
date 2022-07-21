using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Define;

class PacketHandler
{
	public static void S_HandShakeHandler(PacketSession session, IPacket packet)
    {
		Debug.Log("[NetworkManager] @>> RECV : S_HandShake ");
		S_HandShake rPkt = packet as S_HandShake;
		Managers.Player.AddMyPlayer(rPkt.CGUID, rPkt.CGUID.ToString());

		Managers.UI.ClosePopupUI();
		Managers.UI.ShowPopupUI<UI_ConfirmPopup>();
	}
	public static void S_FirstEnterHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_FirstEnter ");
		S_FirstEnter rPkt = packet as S_FirstEnter;

		//Managers.Room.MoveIntroToLobbyRoom(rPkt.CGUID);

		if (rPkt.CGUID != Managers.Player.GetMyCGUID())
			Managers.Player.AddPlayer(rPkt.CGUID, rPkt.playerNickName);

		Managers.UI.ClosePopupUI();
		Managers.Scene.ChangeScene(Define.Scene.LobbyScene);	
	}

	//Recv from Server of All Player (if i Entered this will not excute only other player comin)
	public static void S_AllPlayerListHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_FirstEnter ");
		S_AllPlayerList rPkt = packet as S_AllPlayerList;

		foreach(S_AllPlayerList.OnLinePlayer player in rPkt.onLinePlayers)
        {
			if(player.CGUID != Managers.Player.GetMyCGUID())
				Managers.Player.AddPlayer(player.CGUID, player.playerNickName);
        }
	}

	public static void S_CreateGameRoomHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_CreateGameRoom ");
		S_CreateGameRoom sPkt = packet as S_CreateGameRoom;

		Managers.Room.HandleCreateGameRoom(sPkt);
	}

	public static void S_GetGameRoomsHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_GetGameRooms ");
		S_GetGameRooms sPkt = packet as S_GetGameRooms;

		Managers.Room.HandleGetAllGamrRooms(sPkt);
	}
	public static void S_LobbyToGameHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_GetGameRooms ");
		S_LobbyToGame sPkt = packet as S_LobbyToGame;


	}

	public static void S_GameToLobbyHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_GetGameRooms ");
		S_GameToLobby sPkt = packet as S_GameToLobby;


	}

	//상대방의 채팅을 받았을때
	public static void S_SendChatHandler(PacketSession session, IPacket packet)
    {
		Debug.Log("[NetworkManager] @>> RECV : S_SendChat ");
		S_SendChat rPkt = packet as S_SendChat;
		Managers.Chat.chatAdd(ChatType.Channel, rPkt.nickName, rPkt.chatContent);
	}

	public static void S_NoticeAllHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_NoticeAll ");
	}
	//===============================================================================================
	//===============================================================================================
	//===============================================================================================
	public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_BroadcastEnterGame ");
		S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
		ServerSession serverSession = session as ServerSession;

	}

	public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_BroadcastLeaveGame ");
		S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
		ServerSession serverSession = session as ServerSession;

	}

	public static void S_PlayerListHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_PlayerList ");
		S_PlayerList pkt = packet as S_PlayerList;
		ServerSession serverSession = session as ServerSession;

	}

	public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_BroadcastMove ");
		S_BroadcastMove pkt = packet as S_BroadcastMove;
		ServerSession serverSession = session as ServerSession;

	}

	
	
}
