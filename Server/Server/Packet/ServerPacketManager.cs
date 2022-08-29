using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
		
	public void Register()
	{
		_makeFunc.Add((ushort)PacketID.C_FirstEnter, MakePacket<C_FirstEnter>);
		_handler.Add((ushort)PacketID.C_FirstEnter, PacketHandler.C_FirstEnterHandler);
		_makeFunc.Add((ushort)PacketID.C_CreateGameRoom, MakePacket<C_CreateGameRoom>);
		_handler.Add((ushort)PacketID.C_CreateGameRoom, PacketHandler.C_CreateGameRoomHandler);
		_makeFunc.Add((ushort)PacketID.C_GetGameRooms, MakePacket<C_GetGameRooms>);
		_handler.Add((ushort)PacketID.C_GetGameRooms, PacketHandler.C_GetGameRoomsHandler);
		_makeFunc.Add((ushort)PacketID.C_IntroToLobby, MakePacket<C_IntroToLobby>);
		_handler.Add((ushort)PacketID.C_IntroToLobby, PacketHandler.C_IntroToLobbyHandler);
		_makeFunc.Add((ushort)PacketID.C_LobbyToGame, MakePacket<C_LobbyToGame>);
		_handler.Add((ushort)PacketID.C_LobbyToGame, PacketHandler.C_LobbyToGameHandler);
		_makeFunc.Add((ushort)PacketID.C_ClickReadyOnOff, MakePacket<C_ClickReadyOnOff>);
		_handler.Add((ushort)PacketID.C_ClickReadyOnOff, PacketHandler.C_ClickReadyOnOffHandler);
		_makeFunc.Add((ushort)PacketID.C_GameToLobby, MakePacket<C_GameToLobby>);
		_handler.Add((ushort)PacketID.C_GameToLobby, PacketHandler.C_GameToLobbyHandler);
		_makeFunc.Add((ushort)PacketID.C_SendChat, MakePacket<C_SendChat>);
		_handler.Add((ushort)PacketID.C_SendChat, PacketHandler.C_SendChatHandler);
		_makeFunc.Add((ushort)PacketID.C_LeaveGame, MakePacket<C_LeaveGame>);
		_handler.Add((ushort)PacketID.C_LeaveGame, PacketHandler.C_LeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.C_GameStart, MakePacket<C_GameStart>);
		_handler.Add((ushort)PacketID.C_GameStart, PacketHandler.C_GameStartHandler);
		_makeFunc.Add((ushort)PacketID.C_EnterFieldWorld, MakePacket<C_EnterFieldWorld>);
		_handler.Add((ushort)PacketID.C_EnterFieldWorld, PacketHandler.C_EnterFieldWorldHandler);
		_makeFunc.Add((ushort)PacketID.C_Move, MakePacket<C_Move>);
		_handler.Add((ushort)PacketID.C_Move, PacketHandler.C_MoveHandler);
		_makeFunc.Add((ushort)PacketID.C_WaterBOOM, MakePacket<C_WaterBOOM>);
		_handler.Add((ushort)PacketID.C_WaterBOOM, PacketHandler.C_WaterBOOMHandler);
		_makeFunc.Add((ushort)PacketID.C_GameFinish, MakePacket<C_GameFinish>);
		_handler.Add((ushort)PacketID.C_GameFinish, PacketHandler.C_GameFinishHandler);
		_makeFunc.Add((ushort)PacketID.C_FieldToLobby, MakePacket<C_FieldToLobby>);
		_handler.Add((ushort)PacketID.C_FieldToLobby, PacketHandler.C_FieldToLobbyHandler);
		_makeFunc.Add((ushort)PacketID.C_SkillState, MakePacket<C_SkillState>);
		_handler.Add((ushort)PacketID.C_SkillState, PacketHandler.C_SkillStateHandler);

	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			if (onRecvCallback != null)
				onRecvCallback.Invoke(session, packet);
			else
				HandlePacket(session, packet);
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);
		return pkt;
	}

	public void HandlePacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action.Invoke(session, packet);
	}
}