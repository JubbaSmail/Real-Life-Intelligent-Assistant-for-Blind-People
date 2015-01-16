using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Main_Server
{
    
    class ServerTools
    {
        Socket newserver;
        IPEndPoint iep;
        EndPoint Remote;
        int port;
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

        public ServerTools(int _port)
        {
            port = _port;
            Remote = (EndPoint)(sender);
            iep = new IPEndPoint(IPAddress.Any, port);
            newserver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Connect();
        }

        public bool Connected
        {
            get
            {
                return (newserver != null);
            }
        }

        public void Connect()
        {
            newserver.Bind(iep);
        }

        public void Send(IO_Type type,object[] data)
        {
            byte[] buffer = new byte[22];
            buffer[0] = (byte)type;
            if (type == IO_Type.threeD_Sound)
            {
                short distance = (short)data[0];
                short direction = (short)data[1];
                Array.Copy(BitConverter.GetBytes(distance), 0, buffer, 1, 2);
                Array.Copy(BitConverter.GetBytes(direction), 0, buffer, 3, 2);
            }
            newserver.SendTo(buffer, Remote);
        }

        public object[] Receive()
        {
            byte[] buffer = new byte[22];
            newserver.ReceiveFrom(buffer, ref Remote);
            IO_Type type = (IO_Type)buffer[0];
            byte sync_seconds = buffer[1];
            //Sync Process
            if (type == IO_Type.Command)
            {
                Command_Type command = (Command_Type)buffer[2];
                int i;
                for (i = 2; i < buffer.Length; i++)
                {
                    if (Convert.ToChar(buffer[i]) == ';')
                        break;
                }
                string command_data = Encoding.ASCII.GetString(buffer, 3, i - 3);
                return new object[3] { type, command, command_data };
            }
            else if (type == IO_Type.Location)
            {
                string lats, longs;
                int i;
                for (i = 2; i < buffer.Length; i++)
                {
                    if (Convert.ToChar(buffer[i]) == ';')
                        break;
                }
                lats = Encoding.ASCII.GetString(buffer, 2, i - 2);
                int j;
                for (j = i+1; j < buffer.Length; j++)
                {
                    if (Convert.ToChar(buffer[j]) == ';')
                        break;
                }
                longs = Encoding.ASCII.GetString(buffer, i+1, j - i -1);
                return new object[3] { type, lats ,longs };
            }
            return new object[0];
        }
    }
}