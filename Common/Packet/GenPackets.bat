START ../../Server/PacketGenerator/bin/PacketGenerator.exe ../../Server/PacketGenerator/PDL.xml
XCOPY /Y GenPackets.cs "../../2D_BattleGround/Assets/Scripts/Packet"
XCOPY /Y GenPackets.cs "../../Server/Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../2D_BattleGround/Assets/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../Server/Server/Packet"