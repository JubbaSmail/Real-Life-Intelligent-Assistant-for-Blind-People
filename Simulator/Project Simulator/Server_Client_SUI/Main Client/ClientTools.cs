using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Main_Client
{
    class c_ClientTools
    {
        Socket Sock;
        EndPoint endp;
        string ip;
        int port;

        public c_ClientTools(string _ip,int _port)
        {
            ip = _ip;
            port = _port;
            Connect();
        }

        public void Connect()
        {
            Sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endp = (EndPoint) new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public void Send(IO_Type type, byte sync_seconds, Command_Type command,string command_data)
        {
            if (Sock != null)
            {
                byte[] buffer = new byte[22];
                buffer[0] = (byte)type;
                buffer[1] = sync_seconds;
                buffer[2] = (byte)command;
                command_data += ";";
                Array.Copy(Encoding.ASCII.GetBytes(command_data), 0, buffer, 3, command_data.Length);
                Sock.SendTo(buffer, endp);
            }
        }

        public void Send(IO_Type type, byte sync_seconds, string lats,string longs)
        {
            //check when gps data ok
            if (Sock != null)
            {
                byte[] buffer = new byte[22];
                buffer[0] = (byte)type;
                buffer[1] = sync_seconds;
                lats += ";";
                longs += ";";
                Array.Copy(Encoding.ASCII.GetBytes(lats), 0, buffer, 2, lats.Length);
                Array.Copy(Encoding.ASCII.GetBytes(longs), 0, buffer, 2 + lats.Length, longs.Length);
                Sock.SendTo(buffer, endp);
            }
        }

        public object[] Receive()
        {
            try
            {
                IO_Type type;
                short[] threeD_Sound_data = new short[2];
                byte[] buffer = new byte[22];
                Sock.ReceiveFrom(buffer,ref endp);
                type = (IO_Type)buffer[0];
                if (type == IO_Type.threeD_Sound)
                {
                    threeD_Sound_data[0] = BitConverter.ToInt16(buffer, 1);
                    threeD_Sound_data[1] = BitConverter.ToInt16(buffer, 3);
                }
                return new object[2] { type, threeD_Sound_data };
            }
            catch
            {
                return null;
            }
        }
    }
}