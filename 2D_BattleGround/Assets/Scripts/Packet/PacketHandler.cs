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
		Managers.UI.ClosePopupUI();
		Managers.UI.ShowPopupUI<UI_ConfirmPopup>();
	}
	public static void S_FirstEnterHandler(PacketSession session, IPacket packet)
	{
		Debug.Log("[NetworkManager] @>> RECV : S_FirstEnter ");
		S_FirstEnter Rpkt = packet as S_FirstEnter;
		Managers.Player.AddMyPlayer(Rpkt.CGUID, Rpkt.playerNickName);

		Managers.UI.ClosePopupUI();
		Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
		Managers.Chat.chatAdd(ChatType.System, "", "Entered Channel 1");
	} 

	//상대방의 채팅을 받았을때
	public static void S_SendChatHandler(PacketSession session, IPacket packet)
    {
		Debug.Log("[NetworkManager] @>> RECV : S_SendChat ");
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
