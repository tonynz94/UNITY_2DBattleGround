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
		_makeFunc.Add((ushort)PacketID.S_HandShake, MakePacket<S_HandShake>);
		_handler.Add((ushort)PacketID.S_HandShake, PacketHandler.S_HandShakeHandler);
		_makeFunc.Add((ushort)PacketID.S_FirstEnter, MakePacket<S_FirstEnter>);
		_handler.Add((ushort)PacketID.S_FirstEnter, PacketHandler.S_FirstEnterHandler);
		_makeFunc.Add((ushort)PacketID.S_CreateGameRoom, MakePacket<S_CreateGameRoom>);
		_handler.Add((ushort)PacketID.S_CreateGameRoom, PacketHandler.S_CreateGameRoomHandler);
		_makeFunc.Add((ushort)PacketID.S_GetGameRooms, MakePacket<S_GetGameRooms>);
		_handler.Add((ushort)PacketID.S_GetGameRooms, PacketHandler.S_GetGameRoomsHandler);
		_makeFunc.Add((ushort)PacketID.S_LobbyToGame, MakePacket<S_LobbyToGame>);
		_handler.Add((ushort)PacketID.S_LobbyToGame, PacketHandler.S_LobbyToGameHandler);
		_makeFunc.Add((ushort)PacketID.S_GameToLobby, MakePacket<S_GameToLobby>);
		_handler.Add((ushort)PacketID.S_GameToLobby, PacketHandler.S_GameToLobbyHandler);
		_makeFunc.Add((ushort)PacketID.S_SendChat, MakePacket<S_SendChat>);
		_handler.Add((ushort)PacketID.S_SendChat, PacketHandler.S_SendChatHandler);
		_makeFunc.Add((ushort)PacketID.S_NoticeAll, MakePacket<S_NoticeAll>);
		_handler.Add((ushort)PacketID.S_NoticeAll, PacketHandler.S_NoticeAllHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
		_handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
		_handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
		_handler.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);

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