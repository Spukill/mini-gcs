using System.Net;
using System.Net.Sockets;

namespace MiniGcs.Sniffer;

// Listens for MAVLink packets on UDP port 14550 (where MAVProxy forwards the SITL
// drone's telemetry) and prints every HEARTBEAT: who sent it, what kind of vehicle it
// is, whether it's armed and its flight mode. The bytes are decoded by the generated
// MAVLink library (MiniGcs.Mavlink), which also validates each packet's CRC.
// udpClient.Receive() BLOCKS until a packet arrives; that's fine in a console app,
// but it would freeze a WPF window (that's the problem phase 3 solves).
class Program
{
    private const int MavlinkUdpPort = 14550;

    static void Main(string[] args)
    {
        UdpClient udpClient = new UdpClient(MavlinkUdpPort);
        MAVLink.MavlinkParse parser = new MAVLink.MavlinkParse();

        // Filled in by Receive() with the IP and port of whoever sent the packet.
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

        int packetCount = 0;

        Console.WriteLine("A ouvir MAVLink na porta UDP " + MavlinkUdpPort + "... (Ctrl+C para sair)");

        while (true)
        {
            // 1. Wait for one UDP datagram (blocks here until something arrives).
            byte[] datagram = udpClient.Receive(ref sender);

            // 2. The parser reads from a Stream, so wrap the bytes in a MemoryStream.
            MemoryStream stream = new MemoryStream(datagram);

            // 3. One datagram may contain more than one MAVLink packet: read until the end.
            while (stream.Position < stream.Length)
            {
                MAVLink.MAVLinkMessage? message = ReadNextPacket(parser, stream);

                if (message == null)
                {
                    // Bad CRC, garbage, or an incomplete packet at the end: skip it.
                    break;
                }

                packetCount = packetCount + 1;

                // 4. Only react to HEARTBEAT messages (msgid 0).
                if (message.msgid == (uint)MAVLink.MAVLINK_MSG_ID.HEARTBEAT)
                {
                    PrintHeartbeat(message, packetCount);
                }
            }
        }
    }

    // Reads the next packet from the stream. Returns null if the CRC is wrong or if the
    // stream ends in the middle of a packet (the library throws EndOfStreamException then).
    private static MAVLink.MAVLinkMessage? ReadNextPacket(MAVLink.MavlinkParse parser, MemoryStream stream)
    {
        try
        {
            return parser.ReadPacket(stream);
        }
        catch (EndOfStreamException)
        {
            return null;
        }
    }

    private static void PrintHeartbeat(MAVLink.MAVLinkMessage message, int packetCount)
    {
        // message.data is the payload already decoded into the right struct.
        MAVLink.mavlink_heartbeat_t heartbeat = (MAVLink.mavlink_heartbeat_t)message.data;

        // base_mode is a byte of flags; bit 7 (value 128) = armed. Same idea as checking
        // a flag bit in a frame header with a mask.
        bool isArmed = (heartbeat.base_mode & (byte)MAVLink.MAV_MODE_FLAG.SAFETY_ARMED) != 0;

        string armedText;
        if (isArmed)
        {
            armedText = "ARMED";
        }
        else
        {
            armedText = "DISARMED";
        }

        // custom_mode is just a number (4, 6, ...); casting it to the COPTER_MODE enum
        // and calling ToString() gives the name ("GUIDED", "RTL", ...).
        string flightMode = ((MAVLink.COPTER_MODE)heartbeat.custom_mode).ToString();

        // type says what kind of system sent it (QUADROTOR, GCS, ...).
        string vehicleType = ((MAVLink.MAV_TYPE)heartbeat.type).ToString();

        string time = DateTime.Now.ToString("HH:mm:ss");

        Console.WriteLine("[" + time + "] HEARTBEAT de " + message.sysid + ":" + message.compid
            + " | " + vehicleType
            + " | " + armedText
            + " | " + flightMode
            + " | pacotes recebidos: " + packetCount);
    }
}
