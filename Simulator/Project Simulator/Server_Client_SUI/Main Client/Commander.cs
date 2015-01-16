using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

namespace Main_Client
{
    enum IO_Type { Command, Location, threeD_Sound, You_are_there, OK, Error };
    enum Command_Type { Register, Navigate, Emergency, Stop, None };

    class Commander
    {
        Thread my_thread;
        c_ClientTools ct;
        //----------------------------------------
        byte[] data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050); // send to this
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); //receive from sender.
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        string welcome = "OK";
        EndPoint Remote;
        byte[] result = new byte[1]; // 1 for success , 0 for failure ..
        string message;
        int recv = 0;
        byte[] size = new byte[1];
        //----------------------------------------

        public Commander(string ip,int port)
        {
            ct = new c_ClientTools(ip,port);

            data = Encoding.ASCII.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);// Send OK---------------------
            Remote = (EndPoint)sender;
            server.ReceiveTimeout = 0;
            
            my_thread = new Thread(new ThreadStart(m_SUI_Linker));
            my_thread.IsBackground = true;
            my_thread.Start();
        }

        public void m_send_wait()
        {
            result[0] = 2; //2 for waiting..
            server.SendTo(result, ipep);
        }

        public void m_send_stop_wait()
        {
            result[0] = 4;
            server.SendTo(result, ipep);
        }

        private void m_SUI_Linker()
        {
            while (true)
            {
                recv = server.ReceiveFrom(size, ref Remote);// receive size ...............................
                data = new byte[size[0]];
                recv = server.ReceiveFrom(data, ref Remote);// receive data .................................
                message = Encoding.ASCII.GetString(data, 0, recv);

                if (message.Contains("Register Path "))
                {
                    ct.Send(IO_Type.Command, 0, Command_Type.Register, message.Split(' ')[2]);
                    object[] rec_data = ct.Receive();
                    if ((IO_Type)rec_data[0] == IO_Type.OK)
                    {
                        result[0] = 3; //3 for waiting registering..
                        server.SendTo(result, Remote);
                    }
                    else if ((IO_Type)rec_data[0] == IO_Type.Error)
                    {
                        result[0] = 0;
                        server.SendTo(result, Remote);
                    }
                }
                else if(message.Contains("Take me to "))
                {
                    ct.Send(IO_Type.Command, 0, Command_Type.Navigate, message.Split(' ')[3]);
                    object[] rec_data = ct.Receive();
                    if ((IO_Type)rec_data[0] == IO_Type.OK)
                    {
                        result[0] = 1; //ok
                        server.SendTo(result, Remote);
                    }
                    else if ((IO_Type)rec_data[0] == IO_Type.Error)
                    {
                        result[0] = 0;
                        server.SendTo(result, Remote);
                    }
                }
                else if (message == "Stop ")
                {
                    ct.Send(IO_Type.Command, 0, Command_Type.Stop, ";");
                    object[] rec_data = ct.Receive();
                    if ((IO_Type)rec_data[0] == IO_Type.OK)
                    {
                        result[0] = 1; //1 OK
                        server.SendTo(result, Remote);
                    }
                    else if ((IO_Type)rec_data[0] == IO_Type.Error)
                    {
                        result[0] = 0;
                        server.SendTo(result, Remote);
                    }
                }
                else if (message != "") // if message is valid
                {
                    //execute commands;
                    result[0] = 1;
                    server.SendTo(result, Remote);
                }
            }
        }
    }
}
