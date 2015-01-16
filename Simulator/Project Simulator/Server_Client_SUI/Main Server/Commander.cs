using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace Main_Server
{
    enum IO_Type { Command, Location, threeD_Sound, You_are_there, OK, Error };
    enum Command_Type { Register, Navigate, Emergency, Stop , None };

    class Commander
    {
        Thread my_thread;
        ServerTools st;
        c_Green_Path my_Green_path;

        Command_Type current_status = Command_Type.None;
        Size needed_size;

        public Commander(int port, Size _size)
        {
            needed_size = _size;
            st = new ServerTools(port);
            my_thread = new Thread(new ThreadStart(m_Linker));
            my_thread.IsBackground = true;
            my_thread.Start();
        }

        public bool m_execute(Command_Type command,string command_data)
        {
            switch (command)
            {
                case Command_Type.Register:
                    my_Green_path = new c_Green_Path(command_data);
                    current_status = Command_Type.Register;
                    break;
                case Command_Type.Navigate:
                    my_Green_path = new c_Green_Path(command_data);
                    if (my_Green_path.m_Load_Instance())
                        current_status = Command_Type.Navigate;
                    else
                    {
                        current_status = Command_Type.None;
                        return false;
                    }
                    //load Green_path from here to specific location by name
                    break;
                case Command_Type.Emergency:
                    current_status = Command_Type.Emergency;
                    //call help center
                    break;
                case Command_Type.Stop:
                    if (current_status == Command_Type.Register && my_Green_path != null)
                    {
                        current_status = Command_Type.Stop;
                        return my_Green_path.m_Build_Path();
                    }
                    else
                        current_status = Command_Type.Stop;
                    break;
                //and so on
            }
            return true;
        }

        public void m_new_location(string _lat, string _long)
        {
            switch (current_status)
            {
                case Command_Type.Register:
                    my_Green_path.m_Register_path(_lat, _long);
                    st.Send(IO_Type.OK, null);
                    break;
                case Command_Type.Navigate:
                    object[] data = my_Green_path.m_Navigate(new c_my_Point(Convert.ToDouble(_lat), Convert.ToDouble(_long)),
                        needed_size);
                    st.Send(IO_Type.threeD_Sound, new object[2] { ((short[])data[1])[0], ((short[])data[1])[1] });
                    //((Image)data[0]).Save("test.bmp");
                    m_Visualization((Image)data[0]);
                    break;
                case Command_Type.None:
                    st.Send(IO_Type.OK, null);
                    break;
                default:
                    st.Send(IO_Type.OK, null);
                    break;
            }
        }

        private void m_Linker()
        {
            while (true)
            {
                object[] data = st.Receive();
                IO_Type type = (IO_Type)data[0];
                if (type == IO_Type.Command)
                {
                    if (m_execute((Command_Type)data[1],(string)data[2]))
                        st.Send(IO_Type.OK, null);
                    else
                        st.Send(IO_Type.Error, null);
                }
                else if (type == IO_Type.Location)
                {
                    //Program.main_frm.pic
                    m_new_location((string)data[1], (string)data[2]);
                }
            }
        }

        private void m_Visualization(Image img)
        {
            Program.main_frm.m_Visualization(img);
        }
    }
}