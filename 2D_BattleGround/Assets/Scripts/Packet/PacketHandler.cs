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

	//내가 들어가면 나 포함 모든 플레이어가 받음.
	public static void S_FirstEnterHandler(PacketSession session, IPacket packet)
	{
		
		S_FirstEnter rPkt = packet as S_FirstEnter;

		//Managers.Room.MoveIntroToLobbyRoom(rPkt.CGUID);

		//모든 플레이어가 받기 때문에 나는 이미 넣어줬기 때문에 다른 플레이만 받음.
		if (rPkt.CGUID != Managers.Player.GetMyCGUID()) {
			Managers.Player.AddPlayer(rPkt.CGUID, rPkt.playerNickName);
			Debug.Log($"[NetworkManager] @>> RECV : S_FirstEnter { rPkt.playerNickName} 입장 ");
		}
		else
		{
			Debug.Log("[NetworkManager] @>> RECV : S_FirstEnter : 내가 들어간거에 대해 받아기 때문에 Player에 안넣어줌  ");
		}

	}

	//내가 들어가면 나만 받음.
	public static void S_AllPlayerListHandler(PacketSession session, IPacket packet)
	{

		S_AllPlayerList rPkt = packet as S_AllPlayerList;

		Debug.Log($"[NetworkManager] @>> RECV : S_FirstEnter 현재 나 포함 {rPkt.onLinePlayers.Count} 온라인");
		foreach (S_AllPlayerList.OnLinePlayer player in rPkt.onLinePlayers)
        {
			if(player.CGUID != Managers.Player.GetMyCGUID())
				Managers.Player.AddPlayer(player.CGUID, player.playerNickName);
        }

		Managers.UI.ClosePopupUI();
		Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
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

		Managers.Room.HandleGetAllGameRooms(sPkt);
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
