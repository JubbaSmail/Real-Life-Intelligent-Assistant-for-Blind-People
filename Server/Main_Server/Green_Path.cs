using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Media;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Data;

namespace Main_Server
{
    class c_Green_Path
    {
        private List<c_my_Region> atr_my_Regions = new List<c_my_Region>();
        private List<c_my_Point> atr_Path_Points = new List<c_my_Point>();
        private List<string> atr_Directions = new List<string>();
        private List<double> atr_Angles_in_Degree = new List<double>();
        private List<double> atr_Distances_in_Meters = new List<double>();
        int GPS_accuracy = 3;
        private Size atr_Map_path_size = Size.Empty;
        private int atr_X_Axis_Tranlation;
        private int atr_Y_Axis_Tranlation;
        private int atr_Zoom_value;
        private int atr_Border = 400;
        private int atr_Circle = 2;
        private Font atr_myFont = new Font("", 8);
        private List<c_my_Point>[] atr_Green_Border;
        private List<c_my_Point> atr_Green_Bord_1;
        private List<c_my_Point> atr_Green_Bord_2;
        private List<Point> atr_Blind_People_Path;
        private Pen atr_my_pen = new Pen(Color.Red);
        private Bitmap atr_Map_Image;
        private double atr_all_path_lenght_in_meters = 0;
        Pen pp = new Pen(Color.BlueViolet, 4);
        c_Sound_Beacon atr_sound_beacon = new c_Sound_Beacon();

        private List<c_my_Line> atr_Green_Bord_Line_1;
        private List<c_my_Line> atr_Green_Bord_Line_2;

        enum Direction { Forward, Right, Left };
        int smooth_value = 2;
        string PathName;
        //-----------------------------------------
        public c_Green_Path(string _PathName)
        {
            PathName = _PathName;

            pp.DashStyle = DashStyle.Dash;
            pp.EndCap = LineCap.ArrowAnchor;
        }
        //------------------------------------------
        private c_my_Point[] regionsload = new c_my_Point[4];
        private c_my_Point beaconload = new c_my_Point();
        private c_my_Point PathPointload = new c_my_Point();
        //string map_path = "Output//My_Map_0.bmp";
        //---------------------------DB Connection Parameteres--------------------------
        private SqlConnection conn;
        private SqlCommand command;
        SqlDataReader sdr;
        //-----------------------------------------------------
        public bool m_Save_Instance()
        {
            StringBuilder xmloutput = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = Encoding.UTF8;
            setting.Indent = true;
            setting.IndentChars = "\t";
            setting.OmitXmlDeclaration = true;

            XmlWriter writer = (XmlWriter)XmlTextWriter.Create(xmloutput, setting);
            writer.WriteStartDocument();

            atr_Map_Image.Save(PathName + ".bmp");
            writer.WriteStartElement("GreenPath");
            //-------------------------------------------zoom value----------------------------------
            writer.WriteStartElement("ZoomValue", "");
            writer.WriteString(atr_Zoom_value.ToString());
            writer.WriteEndElement();
            //-------------------------------------------X Translation-------------------------------
            writer.WriteStartElement("XTranslation", "");
            writer.WriteString(atr_X_Axis_Tranlation.ToString());
            writer.WriteEndElement();
            //-------------------------------------------Y Translation-------------------------------
            writer.WriteStartElement("YTranslation", "");
            writer.WriteString(atr_Y_Axis_Tranlation.ToString());
            writer.WriteEndElement();
            //-------------------------------------------Map Image-----------------------------------
            writer.WriteStartElement("MapImage", "");
            writer.WriteString(PathName + ".bmp");
            writer.WriteEndElement();
            //-------------------------------------------Path Point----------------------------------
            writer.WriteStartElement("PathPoint", "");
            writer.WriteStartElement("XPoint", "");
            writer.WriteString(atr_Path_Points[0].atr_Latitude_GPS_coordinate.ToString() + "_"
                + atr_Path_Points[0].atr_X_XY_coordinate.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("YPoint", "");
            writer.WriteString(atr_Path_Points[0].atr_Longitude_GPS_coordinate.ToString() + "_" +
                atr_Path_Points[0].atr_Y_XY_coordinate.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
            //-------------------------------------------Region Count----------------------------------
            writer.WriteStartElement("Count", "");
            writer.WriteString(atr_my_Regions.Count.ToString());
            writer.WriteEndElement();
            //-------------------------------------------Regions-------------------------------------
            writer.WriteStartElement("RegionsList", "");
            writer.WriteStartAttribute("number");
            writer.WriteString(atr_my_Regions.Count.ToString());
            writer.WriteEndAttribute();
            for (int i = 0; i < atr_my_Regions.Count; i++)
            {
                writer.WriteStartElement("Region", "");
                //---------------------------------------Beacon--------------------------------------
                writer.WriteStartElement("Beacon", "");
                writer.WriteStartElement("x", "");
                writer.WriteString(atr_my_Regions[i].atr_Sound_Beacon.atr_X_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("y", "");
                writer.WriteString(atr_my_Regions[i].atr_Sound_Beacon.atr_Y_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("lat", "");
                writer.WriteString(atr_my_Regions[i].atr_Sound_Beacon.atr_Latitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("long", "");
                writer.WriteString(atr_my_Regions[i].atr_Sound_Beacon.atr_Longitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                //---------------------------------------First Point-----------------------------
                writer.WriteStartElement("FirstBorder", "");
                writer.WriteStartElement("x", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[0].atr_X_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("y", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[0].atr_Y_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("lat", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[0].atr_Latitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("long", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[0].atr_Longitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                //---------------------------------------Second Point-----------------------------
                writer.WriteStartElement("SecondBorder", "");
                writer.WriteStartElement("x", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[1].atr_X_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("y", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[1].atr_Y_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("lat", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[1].atr_Latitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("long", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[1].atr_Longitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                //---------------------------------------Third Point-----------------------------
                writer.WriteStartElement("ThirdBorder", "");
                writer.WriteStartElement("x", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[2].atr_X_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("y", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[2].atr_Y_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("lat", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[2].atr_Latitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("long", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[2].atr_Longitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                //---------------------------------------Forth Point-----------------------------
                writer.WriteStartElement("FourthBorder", "");
                writer.WriteStartElement("x", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[3].atr_X_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("y", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[3].atr_Y_XY_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("lat", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[3].atr_Latitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("long", "");
                writer.WriteString(atr_my_Regions[i].atr_Points[3].atr_Longitude_GPS_coordinate.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Close();
            string xmlres = xmloutput.ToString();
            Path p = new Path();
            return p.AddPath(PathName, xmlres);
        }

        public bool m_Load_Instance()
        {
            atr_Blind_People_Path = new List<Point>();
            Path p = new Path();
            Path k = p.GetPathByName(PathName);
            if (k == null)
                return false;
            string oute = string.Empty;
            foreach (char a in k.P_XML)
            {
                if (a != '\r' && a != '\n' && a != '\t')
                {
                    oute += a;
                }
            }
            StringBuilder xmloutput = new StringBuilder();
            XmlReaderSettings settings = new XmlReaderSettings();
            byte[] ba = Encoding.ASCII.GetBytes(oute);
            MemoryStream m = new MemoryStream(ba);
            XmlReader reader = (XmlReader)XmlReader.Create(m);

            atr_Map_Image = (Bitmap)Image.FromFile( PathName + ".bmp");
            reader.ReadStartElement("GreenPath");
            //-------------------------------------------zoom value----------------------------------
            reader.ReadStartElement("ZoomValue", "");
            atr_Zoom_value = int.Parse(reader.ReadString());
            reader.ReadEndElement();
            //-------------------------------------------X Translation-------------------------------
            reader.ReadStartElement("XTranslation", "");
            atr_X_Axis_Tranlation = int.Parse(reader.ReadString());
            reader.ReadEndElement();
            //-------------------------------------------Y Translation-------------------------------
            reader.ReadStartElement("YTranslation", "");
            atr_Y_Axis_Tranlation = int.Parse(reader.ReadString());
            reader.ReadEndElement();
            //-------------------------------------------Map Image-----------------------------------
            reader.ReadStartElement("MapImage", "");
            PathName = reader.ReadString();
            reader.ReadEndElement();
            //-------------------------------------------Path Point----------------------------------
            reader.ReadStartElement("PathPoint", "");
            reader.ReadStartElement("XPoint", "");
            string lat_x = reader.ReadString();
            PathPointload.atr_Latitude_GPS_coordinate = Convert.ToDouble(lat_x.Split('_')[0]);
            PathPointload.atr_X_XY_coordinate = int.Parse(lat_x.Split('_')[1]);
            reader.ReadEndElement();
            reader.ReadStartElement("YPoint", "");
            string long_y = reader.ReadString();
            PathPointload.atr_Longitude_GPS_coordinate = Convert.ToDouble(long_y.Split('_')[0]);
            PathPointload.atr_Y_XY_coordinate = int.Parse(long_y.Split('_')[1]);
            atr_Path_Points.Add(PathPointload);
            reader.ReadEndElement();
            reader.ReadEndElement();
            //-------------------------------------------Region Count----------------------------------
            reader.ReadStartElement("Count", "");
            int regionsnum = int.Parse(reader.ReadString());
            reader.ReadEndElement();
            //-------------------------------------------Regions-------------------------------------

            reader.ReadStartElement("RegionsList", "");
            for (int i = 0; i < regionsnum; i++)
            {
                regionsload = new c_my_Point[4];
                regionsload[0] = new c_my_Point();
                regionsload[1] = new c_my_Point();
                regionsload[2] = new c_my_Point();
                regionsload[3] = new c_my_Point();
                beaconload = new c_my_Point();
                reader.ReadStartElement("Region", "");
                //---------------------------------------Beacon--------------------------------------
                reader.ReadStartElement("Beacon", "");
                reader.ReadStartElement("x", "");
                beaconload.atr_X_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("y", "");
                beaconload.atr_Y_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("lat", "");
                beaconload.atr_Latitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("long", "");
                beaconload.atr_Longitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadEndElement();
                //---------------------------------------First Point-----------------------------
                reader.ReadStartElement("FirstBorder", "");
                reader.ReadStartElement("x", "");
                regionsload[0].atr_X_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("y", "");
                regionsload[0].atr_Y_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("lat", "");
                regionsload[0].atr_Latitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("long", "");
                regionsload[0].atr_Longitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadEndElement();
                //---------------------------------------Second Point-----------------------------
                reader.ReadStartElement("SecondBorder", "");
                reader.ReadStartElement("x", "");
                regionsload[1].atr_X_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("y", "");
                regionsload[1].atr_Y_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("lat", "");
                regionsload[1].atr_Latitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("long", "");
                regionsload[1].atr_Longitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadEndElement();
                //---------------------------------------Third Point-----------------------------
                reader.ReadStartElement("ThirdBorder", "");
                reader.ReadStartElement("x", "");
                regionsload[2].atr_X_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("y", "");
                regionsload[2].atr_Y_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("lat", "");
                regionsload[2].atr_Latitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("long", "");
                regionsload[2].atr_Longitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadEndElement();
                //---------------------------------------Forth Point-----------------------------
                reader.ReadStartElement("FourthBorder", "");
                reader.ReadStartElement("x", "");
                regionsload[3].atr_X_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("y", "");
                regionsload[3].atr_Y_XY_coordinate = int.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("lat", "");
                regionsload[3].atr_Latitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadStartElement("long", "");
                regionsload[3].atr_Longitude_GPS_coordinate = double.Parse(reader.ReadString());
                reader.ReadEndElement();
                reader.ReadEndElement();
                reader.ReadEndElement();
                atr_my_Regions.Add(new c_my_Region(regionsload, beaconload));
            }
            reader.ReadEndElement();
            reader.ReadEndElement();
            reader.Close();
            return true;
        }
        //-----------------------------------------------------
        public bool m_Build_Path()
        {
            if (_all_lats.Count > 0 && _all_longs.Count > 0)
            {
                object[] data = m_Direction_Generator_and_Smoother(_all_lats, _all_longs);
                return m_Build((string[])data[0], (string[])data[1], (string[])data[2], 15);
            }
            return false;
        }

        bool m_Build(string[] _all_lats, string[] _all_longs, string[] _my_directions, int _Zoom_value)
        {
            try
            {
                atr_Blind_People_Path = new List<Point>();
                try
                {
                    conn = new SqlConnection(Client.connstring);
                    command = new SqlCommand("dbo.GetPathByName", conn);
                    command.Connection.Open();
                }
                catch (System.Exception ex)
                {
                    return false;
                }
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@P_Name", PathName));
                    sdr = command.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    return false;
                }

                if (sdr.HasRows)
                {
                    sdr.Close();
                    conn.Close();
                    m_Load_Instance();
                    return true;
                }
                else
                {
                    sdr.Close();
                    conn.Close();
                    atr_Zoom_value = _Zoom_value;

                    string[] all_lats = _all_lats;
                    string[] all_longs = _all_longs;
                    string[] my_directions = _my_directions;
                    

                    //Step 1:
                    if (m_Translate_Points_to_Suitable_Position(all_lats, all_longs))
                    {
                        for (int i = 0; i < my_directions.Length; i++)
                        {
                            if (my_directions != null)
                            {
                                if (my_directions[i] != string.Empty)
                                {
                                    string[] dir = my_directions[i].Split(':');

                                    this.atr_Directions.Add(dir[0]);
                                    if (dir.Length > 1)
                                    {
                                        this.atr_Angles_in_Degree.Add(Convert.ToDouble(dir[1]));

                                        this.atr_Distances_in_Meters.Add
                                            (this.m_Get_Distance_in_Meters(this.atr_Path_Points[i].m_My_Clone(),
                                            this.atr_Path_Points[i - 1].m_My_Clone()));

                                        atr_all_path_lenght_in_meters += this.atr_Distances_in_Meters[i];
                                    }
                                    else
                                    {
                                        this.atr_Angles_in_Degree.Add(double.NaN);
                                        this.atr_Distances_in_Meters.Add(double.NaN);
                                    }
                                }
                            }
                        }
                        //Step 2:
                        if (m_Build_Green_Region(GPS_accuracy) &&
                            //Step 3:
                        m_Build_Regions() &&
                            //Step 4:
                        m_Build_Map_Image() &&
                            //Step 5:
                        m_Save_Instance()
                            )
                            return true;
                        return false;
                    }
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public Image m_Get_Map_Image()
        {
            if (atr_Blind_People_Path.Count > 1)
                Graphics.FromImage(atr_Map_Image).DrawCurve(pp, atr_Blind_People_Path.ToArray());
            return atr_Map_Image;
        }

        public Image m_Get_Map_Image(Image background, c_my_Point current_point, Size image_size_needed)
        {
            current_point.atr_X_XY_coordinate = current_point.atr_X_XY_coordinate - (image_size_needed.Width / 2);
            current_point.atr_Y_XY_coordinate = current_point.atr_Y_XY_coordinate - (image_size_needed.Height / 2);
            Image needed_image = new Bitmap(image_size_needed.Width, image_size_needed.Height);

            if (background != null)
                Graphics.FromImage(needed_image).DrawImage(background, -current_point.atr_X_XY_coordinate, -current_point.atr_Y_XY_coordinate);

            Graphics.FromImage(needed_image).DrawImage(atr_Map_Image, -current_point.atr_X_XY_coordinate, -current_point.atr_Y_XY_coordinate);

            Graphics.FromImage(needed_image).DrawEllipse(Pens.Black,
    new Rectangle((image_size_needed.Width / 2) - 10,
        (image_size_needed.Height / 2) - 10, 20, 20));

            Graphics.FromImage(needed_image).DrawEllipse(Pens.Red,
                new Rectangle((image_size_needed.Width / 2) - 5,
                    (image_size_needed.Height / 2) - 5, 10, 10));

            return needed_image;
        }

        int old_index_of_nearst_region = -2;
        int step_counter = 0;
        bool open_navigate = false;
        c_my_Point point_1 = new c_my_Point();
        c_my_Point point_4 = new c_my_Point();

        public object[] m_Navigate(c_my_Point current_position, Size image_size_needed)
        {
            short[] navigate_data = null;
            
            if (current_position.atr_X_XY_coordinate == 0)
                current_position = m_Convert_GPS_Coordinate_to_XY_Coordinate
                (current_position.atr_Latitude_GPS_coordinate, current_position.atr_Longitude_GPS_coordinate);

            step_counter++;
            if (step_counter == 3)
            {
                open_navigate = true;
                step_counter = 0;
                point_4 = current_position.m_My_Clone();
            }
            else
            {
                if (step_counter == 1)
                    point_1 = current_position.m_My_Clone();
                open_navigate = false;
            }


            atr_Blind_People_Path.Add(current_position.m_Get_default_Point());

            Image copy_of = new Bitmap(image_size_needed.Width * 2, image_size_needed.Height * 2);

            Graphics.FromImage(copy_of).DrawRectangle(Pens.Black, 1, 1, copy_of.Width - 2, copy_of.Height - 2);

            //Nearst region
            int index_of_nearst_region = 0;
            int min_distance = int.MaxValue;
            int temp;
            for (int i = 0; i < atr_my_Regions.Count; i++)
            {
                temp = atr_my_Regions[i].m_distance_from_In_XY_coordinate(current_position);
                if (temp < min_distance)
                {
                    min_distance = temp;
                    index_of_nearst_region = i;
                }
            }

            if (atr_my_Regions[index_of_nearst_region].m_Is_In(current_position))
            {
                //Draw Current Region
                Point[] region_temp = atr_my_Regions[index_of_nearst_region].m_Get_defualt_Points();
                List<Point> region_temp1 = new List<Point>();
                foreach (var p in region_temp)
                {
                    region_temp1.Add(p);
                }
                region_temp1.Insert(2, atr_my_Regions[index_of_nearst_region].atr_Sound_Beacon.m_Get_default_Point());

                Graphics.FromImage(copy_of).FillPolygon(Brushes.GreenYellow, region_temp1.ToArray());

                Graphics.FromImage(copy_of).FillEllipse(Brushes.Red,
                new Rectangle(atr_my_Regions[index_of_nearst_region].atr_Sound_Beacon.m_Get_default_Point().X - 10,
                     atr_my_Regions[index_of_nearst_region].atr_Sound_Beacon.m_Get_default_Point().Y - 10, 20, 20));
                //------------------------
                //Inside Green Region
                //say "Forward", it is ok here
                //navigate_data = atr_sound_beacon.m_Active_Sound_Beacon(open_navigate);
                navigate_data = atr_sound_beacon.m_Active_Sound_Beacon
                            (open_navigate,
                            point_1.m_Get_default_Point(),
                            point_4.m_Get_default_Point()
                            , atr_my_Regions[index_of_nearst_region].atr_Sound_Beacon.m_Get_default_Point());

                old_index_of_nearst_region = index_of_nearst_region;
            }
            else
            {
                if (index_of_nearst_region + 1 == atr_my_Regions.Count)
                {
                    //you are there
                    navigate_data = new short[2] { -1, -1 };
                    index_of_nearst_region--;
                }
                else if (atr_my_Regions[index_of_nearst_region + 1].m_Is_In(current_position))
                {
                    if (old_index_of_nearst_region == index_of_nearst_region
                        && index_of_nearst_region + 1 != atr_my_Regions.Count)
                    {
                        //when move from old region to new region say "Direction[i]"
                        //Inside Green Region
                        if (index_of_nearst_region == 0)
                        {
                            navigate_data = atr_sound_beacon.m_Active_Sound_Beacon
                            (open_navigate,
                            point_1.m_Get_default_Point(),
                            point_4.m_Get_default_Point()
                            , atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());
                            /*navigate_data = atr_sound_beacon.m_Active_Sound_Beacon(true,
                                atr_Path_Points[0].m_Get_default_Point(),
    atr_my_Regions[index_of_nearst_region].atr_Sound_Beacon.m_Get_default_Point(),
    atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());*/
                        }
                        else
                        {
                            navigate_data = atr_sound_beacon.m_Active_Sound_Beacon
                            (open_navigate,
                            point_1.m_Get_default_Point(),
                            point_4.m_Get_default_Point()
                            , atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());
                            /*navigate_data = atr_sound_beacon.m_Active_Sound_Beacon(true,
    atr_my_Regions[index_of_nearst_region - 1].atr_Sound_Beacon.m_Get_default_Point(),
    atr_my_Regions[index_of_nearst_region].atr_Sound_Beacon.m_Get_default_Point(),
    atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());*/
                        }
                        old_index_of_nearst_region = index_of_nearst_region + 1;
                        step_counter = 0;
                    }
                    else
                    {
                        //Inside Green Region
                        //say "Forward", it is ok here
                        //navigate_data = atr_sound_beacon.m_Active_Sound_Beacon(open_navigate);
                        navigate_data = atr_sound_beacon.m_Active_Sound_Beacon
                            (open_navigate,
                            point_1.m_Get_default_Point(),
                            point_4.m_Get_default_Point()
                            , atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());
                    }
                }
                else
                {
                    //Call Sound Beacon
                    // Out of the whole Region

                    navigate_data = atr_sound_beacon.m_Active_Sound_Beacon
                            (open_navigate,
                            point_1.m_Get_default_Point(),
                            point_4.m_Get_default_Point()
                            , atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());
                }


                Point[] region_temp = atr_my_Regions[index_of_nearst_region + 1].m_Get_defualt_Points();
                List<Point> region_temp1 = new List<Point>();
                foreach (var p in region_temp)
                {
                    region_temp1.Add(p);
                }
                region_temp1.Insert(2, atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point());

                Graphics.FromImage(copy_of).FillPolygon(Brushes.GreenYellow, region_temp1.ToArray());

                Graphics.FromImage(copy_of).FillEllipse(Brushes.Red,
            new Rectangle(atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point().X - 10,
                 atr_my_Regions[index_of_nearst_region + 1].atr_Sound_Beacon.m_Get_default_Point().Y - 10, 20, 20));
            }
            if (atr_Blind_People_Path.Count > 1)
                Graphics.FromImage(copy_of).DrawCurve(pp, atr_Blind_People_Path.ToArray());
            Image result = m_Get_Map_Image(copy_of, current_position, image_size_needed);
            copy_of.Dispose();
            GC.Collect();
            return new object[2] { result, navigate_data };
        }

        //-----------------------------------------------------

        List<string> _all_lats = new List<string>();
        List<string> _all_longs = new List<string>();
        public void m_Register_path(string _lat, string _long)
        {
            //you should clear the List, the "." and last 3 points.
            _all_lats.Add(_lat);
            _all_longs.Add(_long);
        }

        public object[] m_Direction_Generator_and_Smoother(List<string> all_lats, List<string> all_longs)
        {
            List<string> all_directions = new List<string>();
            List<string> new_all_lats = new List<string>();
            List<string> new_all_longs = new List<string>();
            if (all_lats[0].IndexOf('.') != -1 && all_lats[0].Split('.')[1].Length == 5)
            {
                for (int i = 0; i < all_lats.Count; i++)
                {
                    all_lats[i] = all_lats[i].Split('.')[0] + all_lats[i].Split('.')[1].Remove(3, 2);
                    all_longs[i] = all_longs[i].Split('.')[0] + all_longs[i].Split('.')[1].Remove(3, 2);
                }
            }
            int x1, x2, x3, y1, y2, y3;
            double d1, d2, d3, Cos_alpha;


            int step = smooth_value;
            all_directions.Add("S");
            for (int i = step; i + step < all_lats.Count; i += step)
            {
                if (all_lats[i - step].IndexOf('.') > -1)
                {
                    all_lats[i - step] = all_lats[i - step].Remove(all_lats[i - step].IndexOf('.'), 1);
                    all_longs[i - step] = all_longs[i - step].Remove(all_longs[i - step].IndexOf('.'), 1);
                }
                if (all_lats[i].IndexOf('.') > -1)
                {
                    all_lats[i] = all_lats[i].Remove(all_lats[i].IndexOf('.'), 1);
                    all_longs[i] = all_longs[i].Remove(all_longs[i].IndexOf('.'), 1);


                }
                if (all_lats[i + step].IndexOf('.') > -1)
                {
                    all_lats[i + step] = all_lats[i + step].Remove(all_lats[i + step].IndexOf('.'), 1);
                    all_longs[i + step] = all_longs[i + step].Remove(all_longs[i + step].IndexOf('.'), 1);
                }
                x1 = Convert.ToInt32(all_lats[i - step]);
                y1 = Convert.ToInt32(all_longs[i - step]);

                x2 = Convert.ToInt32(all_lats[i]);
                y2 = Convert.ToInt32(all_longs[i]);

                x3 = Convert.ToInt32(all_lats[i + step]);
                y3 = Convert.ToInt32(all_longs[i + step]);

                //Check that p1 & p2 & p3 are not equal
                if (!((x1 == x2 && y1 == y2) ||
                    (x1 == x3 && y1 == y3) ||
                    (x2 == x3 && y2 == y3)))
                {
                    new_all_lats.Add(all_lats[i]);
                    new_all_longs.Add(all_longs[i]);
                }
            }

            for (int i = 1; i + 1 < new_all_lats.Count; i++)
            {
                //take care with this
                //P1
                x1 = Convert.ToInt32(new_all_lats[i - 1]);
                y1 = Convert.ToInt32(new_all_longs[i - 1]);
                //P2
                x2 = Convert.ToInt32(new_all_lats[i]);
                y2 = Convert.ToInt32(new_all_longs[i]);
                //P3
                x3 = Convert.ToInt32(new_all_lats[i + 1]);
                y3 = Convert.ToInt32(new_all_longs[i + 1]);

                //Check that p1 & p2 & p3 are not equal

                d1 = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
                d2 = Math.Sqrt(Math.Pow(y3 - y2, 2) + Math.Pow(x3 - x2, 2));
                d3 = Math.Sqrt(Math.Pow(y3 - y1, 2) + Math.Pow(x3 - x1, 2));
                //Shall Formula
                Cos_alpha = (-Math.Pow(d3, 2) + Math.Pow(d1, 2) + Math.Pow(d2, 2)) /
                    (2 * d1 * d2);
                //Cos_alpha in [-1,1]  Cos(180)= -1 Cos(90)= 0   Cos(0) = 1
                if (Cos_alpha > 1)
                    Cos_alpha = 1;
                else if (Cos_alpha < -1)
                    Cos_alpha = -1;
                double alpha_radian = Math.Acos(Cos_alpha);
                double alpha_degree = Math.Round((alpha_radian * 180) / Math.PI);//convert to degree
                if (Cos_alpha <= -0.99)//[-1 , - 0.7]
                {
                    all_directions.Add(Direction.Forward.ToString() + ":" + alpha_degree.ToString());
                }
                else //either Left or Right
                {
                    //translate to (0,0)

                    int X1, X2, X3, Y1, Y2, Y3;
                    int mx = 0, my = 0;
                    X1 = mx;
                    Y1 = my;

                    my += x2 - x1;
                    mx += y2 - y1;

                    X2 = mx;
                    Y2 = my;

                    my += x3 - x2;
                    mx += y3 - y2;

                    X3 = mx;
                    Y3 = my;

                    x1 = X1; x2 = X2; x3 = X3;
                    y1 = Y1; y2 = Y2; y3 = Y3;


                    // where is the point 2? it is in part (1 | 2 | 3 | 4)
                    if (x2 >= 0)
                    {
                        // in part 1 | 4
                        if (x2 == 0)
                        {
                            if (x3 > 0)
                            {
                                if (y2 > 0)
                                    all_directions.Add(Direction.Right.ToString() + ":" + alpha_degree.ToString());
                                else
                                    all_directions.Add(Direction.Left.ToString() + ":" + alpha_degree.ToString());
                            }
                            else
                            {
                                if (y2 > 0)
                                    all_directions.Add(Direction.Left.ToString() + ":" + alpha_degree.ToString());
                                else
                                    all_directions.Add(Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            }
                        }
                        else
                        {
                            float slope = (float)y2 / (float)x2;
                            //in part 1 | 4
                            if (slope * x3 > y3)
                                all_directions.Add(Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            else
                                all_directions.Add(Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        }
                    }
                    else
                    {
                        // in part 2 | 3
                        float slope = (float)y2 / (float)x2;
                        if (slope * x3 > y3)
                            all_directions.Add(Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        else
                            all_directions.Add(Direction.Right.ToString() + ":" + alpha_degree.ToString());
                    }
                }
            }
            all_directions.Add("E");
            return new object[3] { new_all_lats.ToArray(), new_all_longs.ToArray(), all_directions.ToArray() };
        }

        public void m_Register_location()
        {
        }
        //-----------------------------------------------------

        private bool m_Translate_Points_to_Suitable_Position(string[] all_lats, string[] all_longs)
        {
            try
            {
                int x = 0;
                int y = 0;
                int x_latr_1, y_long_1;
                if (all_lats[0].IndexOf('.') != -1)
                {
                    int index_of_point_lats = all_lats[0].IndexOf('.');
                    int index_of_point_longs = all_longs[0].IndexOf('.');
                    for (int qi = 0; qi < all_lats.Length; qi++)
                    {
                        all_lats[qi] = all_lats[qi].Remove(index_of_point_lats, 1);
                        all_longs[qi] = all_longs[qi].Remove(index_of_point_longs, 1);
                        all_lats[qi] = all_lats[qi].Remove(7, 2);
                        all_longs[qi] = all_longs[qi].Remove(8, 2);
                    }
                }
                x_latr_1 = Convert.ToInt32(all_lats[0]);
                y_long_1 = Convert.ToInt32(all_longs[0]);
                List<Point> my_points = new List<Point>();
                my_points.Add(new Point(x, y));
                int x_latr_2, y_long_2;
                int Max_X = 0, Max_X_index = 0;
                int Min_X = 0, Min_X_index = 0;
                int Max_Y = 0, Max_Y_index = 0;
                int Min_Y = 0, Min_Y_index = 0;
                for (int i = 1; i < all_lats.Length; i++)
                {
                    x_latr_2 = Convert.ToInt32(all_lats[i]);
                    y_long_2 = Convert.ToInt32(all_longs[i]);

                    y -= (x_latr_2 - x_latr_1) * atr_Zoom_value;
                    x += (y_long_2 - y_long_1) * atr_Zoom_value;

                    if (x > Max_X)
                    {
                        Max_X = x;
                        Max_X_index = i;
                    }
                    if (x < Min_X)
                    {
                        Min_X = x;
                        Min_X_index = i;
                    }
                    if (y > Max_Y)
                    {
                        Max_Y = y;
                        Max_Y_index = i;
                    }
                    if (y < Min_Y)
                    {
                        Min_Y = y;
                        Min_Y_index = i;
                    }
                    my_points.Add(new Point(x, y));
                    x_latr_1 = x_latr_2;
                    y_long_1 = y_long_2;
                }

                this.atr_Map_path_size = new Size((Max_X - Min_X) + atr_Border, (Max_Y - Min_Y) + atr_Border);

                int translation_on_x_axis = my_points[Min_X_index].X - (atr_Border / 2);
                int translation_on_y_axis = my_points[Min_Y_index].Y - (atr_Border / 2);

                this.atr_X_Axis_Tranlation = translation_on_x_axis;
                this.atr_Y_Axis_Tranlation = translation_on_y_axis;
                for (int i = 0; i < my_points.Count; i++)
                {
                    my_points[i] = new Point(my_points[i].X - translation_on_x_axis,
                        my_points[i].Y - translation_on_y_axis);
                    this.atr_Path_Points.Add(new c_my_Point(my_points[i].X, my_points[i].Y, Convert.ToInt32(all_lats[i]), Convert.ToInt32(all_longs[i])));
                }
                return true;
            }
            catch { return false; }
        }

        private bool m_Build_Green_Region(int Circle_Radius_in_Meters)
        {
            try
            {
                atr_Green_Bord_1 = new List<c_my_Point>();
                atr_Green_Bord_2 = new List<c_my_Point>();
                atr_Green_Bord_Line_1 = new List<c_my_Line>();
                atr_Green_Bord_Line_2 = new List<c_my_Line>();
                int Circle_Radius_in_GPS = m_Convert_Distance_in_Meter_to_Distance_in_GPS_approximately(Circle_Radius_in_Meters);

                //double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                string direction1;
                Point p1, p2, p3 = new Point();
                PointF pr1, pr2, pl1, pl2;
                for (int i = 0; i < atr_Path_Points.Count; i++)
                {
                    if (i == atr_Path_Points.Count - 1)
                    {
                        p1 = new Point((int)atr_Path_Points[i - 1].atr_Latitude_GPS_coordinate,
                            (int)atr_Path_Points[i - 1].atr_Longitude_GPS_coordinate);

                        p2 = new Point((int)atr_Path_Points[i].atr_Latitude_GPS_coordinate,
                            (int)atr_Path_Points[i].atr_Longitude_GPS_coordinate);

                        int x_translation = p1.X;
                        int y_translation = p1.Y;
                        //Step 1:
                        p1.X -= x_translation;
                        p1.Y -= y_translation;

                        p2.X -= x_translation;
                        p2.Y -= y_translation;

                        p3.X = -Circle_Radius_in_GPS;
                        p3.Y = 0;

                        pr1 = new Point(0, -Circle_Radius_in_GPS);
                        pr2 = new Point(-Circle_Radius_in_GPS, -Circle_Radius_in_GPS);

                        pl1 = new Point(0, Circle_Radius_in_GPS);
                        pl2 = new Point(-Circle_Radius_in_GPS, Circle_Radius_in_GPS);

                        //Step 2:

                        direction1 = m_Get_Direction_and_Degree(p3, p1, p2);
                        double theta = Convert.ToDouble(direction1.Split(':')[1]);
                        if (direction1[0] == 'R')
                            theta *= -1;

                        pr1 = m_Rotate(pr1, theta);
                        pr2 = m_Rotate(pr2, theta);
                        pl1 = m_Rotate(pl1, theta);
                        pl2 = m_Rotate(pl2, theta);

                        //Step 3:
                        atr_Green_Bord_Line_1.Add(new c_my_Line(pr1, pr2));
                        atr_Green_Bord_Line_2.Add(new c_my_Line(pl1, pl2));
                    }
                    else
                    {
                        p1 = new Point((int)atr_Path_Points[i].atr_Latitude_GPS_coordinate,
                            (int)atr_Path_Points[i].atr_Longitude_GPS_coordinate);
                        p2 = new Point((int)atr_Path_Points[i + 1].atr_Latitude_GPS_coordinate,
                            (int)atr_Path_Points[i + 1].atr_Longitude_GPS_coordinate);

                        int x_translation = p1.X;
                        int y_translation = p1.Y;
                        //Step 1:
                        p1.X -= x_translation;
                        p1.Y -= y_translation;

                        p2.X -= x_translation;
                        p2.Y -= y_translation;

                        p3.X = -Circle_Radius_in_GPS;
                        p3.Y = 0;

                        pr1 = new Point(0, -Circle_Radius_in_GPS);
                        pr2 = new Point(-Circle_Radius_in_GPS, -Circle_Radius_in_GPS);

                        pl1 = new Point(0, Circle_Radius_in_GPS);
                        pl2 = new Point(-Circle_Radius_in_GPS, Circle_Radius_in_GPS);

                        //Step 2:

                        direction1 = m_Get_Direction_and_Degree(p3, p1, p2);
                        double theta = Convert.ToDouble(direction1.Split(':')[1]);
                        if (direction1[0] == 'R')
                            theta *= -1;

                        pr1 = m_Rotate(pr1, theta);
                        pr2 = m_Rotate(pr2, theta);
                        pl1 = m_Rotate(pl1, theta);
                        pl2 = m_Rotate(pl2, theta);

                        //Step 3:
                        atr_Green_Bord_Line_1.Add(new c_my_Line(pr1, pr2));
                        atr_Green_Bord_Line_2.Add(new c_my_Line(pl1, pl2));
                    }
                }

                for (int i = 0; i < atr_Green_Bord_Line_1.Count - 1; i++)
                {
                    p1 = new Point((int)atr_Path_Points[i].atr_Latitude_GPS_coordinate, (int)atr_Path_Points[i].atr_Longitude_GPS_coordinate);
                    p2 = new Point((int)atr_Path_Points[i + 1].atr_Latitude_GPS_coordinate, (int)atr_Path_Points[i + 1].atr_Longitude_GPS_coordinate);

                    if (i == 0)
                    {
                        atr_Green_Bord_1.Add(m_Convert_GPS_Coordinate_to_XY_Coordinate(atr_Green_Bord_Line_1[i].p1.X + p1.X, atr_Green_Bord_Line_1[i].p1.Y + p1.Y));
                        atr_Green_Bord_2.Add(m_Convert_GPS_Coordinate_to_XY_Coordinate(atr_Green_Bord_Line_2[i].p1.X + p1.X, atr_Green_Bord_Line_2[i].p1.Y + p1.Y));
                    }

                    PointF p_intersect1 = PointF.Empty, p_intersect2 = PointF.Empty;

                    c_my_Line Line_1a = atr_Green_Bord_Line_1[i].m_Clone();
                    c_my_Line Line_2a = atr_Green_Bord_Line_1[i + 1].m_Clone();

                    c_my_Line Line_1b = atr_Green_Bord_Line_2[i].m_Clone();
                    c_my_Line Line_2b = atr_Green_Bord_Line_2[i + 1].m_Clone();

                    int shift_x = p2.X - p1.X;
                    int shift_y = p2.Y - p1.Y;

                    Line_2a.p1.X += shift_x;
                    Line_2a.p1.Y += shift_y;

                    Line_2a.p2.X += shift_x;
                    Line_2a.p2.Y += shift_y;
                    //--------------------------
                    Line_2b.p1.X += shift_x;
                    Line_2b.p1.Y += shift_y;

                    Line_2b.p2.X += shift_x;
                    Line_2b.p2.Y += shift_y;
                    //--------------------------

                    if (Line_1a.m_test_line() && Line_2a.m_test_line())
                    {
                        p_intersect1 = Line_1a.m_Intersect(Line_2a);
                        p_intersect2 = Line_1b.m_Intersect(Line_2b);
                    }
                    else
                    {
                        double new_rotated = 0, value_of_rotate = 5;
                        while (!(Line_1a.m_test_line() && Line_2a.m_test_line()))
                        {
                            new_rotated += value_of_rotate;
                            Line_1a.p1 = m_Rotate_Axis(Line_1a.p1, value_of_rotate);
                            Line_1a.p2 = m_Rotate_Axis(Line_1a.p2, value_of_rotate);

                            Line_1b.p1 = m_Rotate_Axis(Line_1b.p1, value_of_rotate);
                            Line_1b.p2 = m_Rotate_Axis(Line_1b.p2, value_of_rotate);
                            //------------------------------------------------------
                            Line_2a.p1 = m_Rotate_Axis(Line_2a.p1, value_of_rotate);
                            Line_2a.p2 = m_Rotate_Axis(Line_2a.p2, value_of_rotate);

                            Line_2b.p1 = m_Rotate_Axis(Line_2b.p1, value_of_rotate);
                            Line_2b.p2 = m_Rotate_Axis(Line_2b.p2, value_of_rotate);
                        }
                        p_intersect1 = Line_1a.m_Intersect(Line_2a);
                        p_intersect2 = Line_1b.m_Intersect(Line_2b);
                        //re-rotate
                        p_intersect1 = m_Rotate_Axis(p_intersect1, -new_rotated);
                        p_intersect2 = m_Rotate_Axis(p_intersect2, -new_rotated);
                    }

                    p_intersect1.X += p1.X;
                    p_intersect1.Y += p1.Y;

                    p_intersect2.X += p1.X;
                    p_intersect2.Y += p1.Y;


                    atr_Green_Bord_1.Add(m_Convert_GPS_Coordinate_to_XY_Coordinate(p_intersect1.X, p_intersect1.Y));
                    atr_Green_Bord_2.Add(m_Convert_GPS_Coordinate_to_XY_Coordinate(p_intersect2.X, p_intersect2.Y));

                }
                atr_Green_Border = new List<c_my_Point>[2] { atr_Green_Bord_1, atr_Green_Bord_2 };
                return true;
            }
            catch
            { return false; }
        }

        private PointF m_Rotate(PointF p, double theta)
        {
            theta = theta * Math.PI / 180;
            PointF p_rotated = new Point();

            p_rotated.X = (float)((p.X * Math.Cos(theta)) - (p.Y * Math.Sin(theta)));
            p_rotated.Y = (float)((p.X * Math.Sin(theta)) + (p.Y * Math.Cos(theta)));
            return p_rotated;
        }

        private PointF m_Rotate_Axis(PointF p, double theta)
        {
            theta = theta * Math.PI / 180;
            PointF p_rotated = new Point();

            p_rotated.X = (float)((p.X * Math.Cos(theta)) + (p.Y * Math.Sin(theta)));
            p_rotated.Y = (float)(-(p.X * Math.Sin(theta)) + (p.Y * Math.Cos(theta)));
            return p_rotated;
        }

        private bool m_Build_Regions()
        {
            try
            {
                for (int i = 1; i < atr_Path_Points.Count; i++)
                {
                    atr_my_Regions.Add(
                        new c_my_Region(
                            new c_my_Point[4]
                {atr_Green_Bord_1[i-1],atr_Green_Bord_1[i],
                atr_Green_Bord_2[i],atr_Green_Bord_2[i-1]}
                    , atr_Path_Points[i]
                    ));
                }
                return true;
            }
            catch { return false; }
        }

        private bool m_Build_Map_Image()
        {
            try
            {
                Bitmap my_map = new Bitmap(this.atr_Map_path_size.Width, this.atr_Map_path_size.Height);

                for (int i = 0; i < this.atr_Path_Points.Count; i++)
                {
                    Graphics.FromImage(my_map).DrawEllipse(Pens.Blue, this.atr_Path_Points[i].atr_X_XY_coordinate - atr_Circle, this.atr_Path_Points[i].atr_Y_XY_coordinate - atr_Circle, 2 * atr_Circle, 2 * atr_Circle);

                    if (this.atr_Directions.Count > 0 && this.atr_Directions[i] != string.Empty)
                    {
                        Graphics.FromImage(my_map).DrawString(this.atr_Directions[i][0].ToString(), atr_myFont, Brushes.Black, this.atr_Path_Points[i].atr_X_XY_coordinate, this.atr_Path_Points[i].atr_Y_XY_coordinate);

                        if (this.atr_Angles_in_Degree.Count > 0)
                        {
                            if (i != 0 && i != atr_Angles_in_Degree.Count - 1)
                            {
                                Graphics.FromImage(my_map).DrawString(" : " + atr_Angles_in_Degree[i] + " Deg ", atr_myFont, Brushes.Black, this.atr_Path_Points[i].atr_X_XY_coordinate + 10, this.atr_Path_Points[i].atr_Y_XY_coordinate);

                                Graphics.FromImage(my_map).DrawString
                                    (" : " + this.atr_Distances_in_Meters[i] + " M", atr_myFont, Brushes.Black,
                                    this.atr_Path_Points[i].atr_X_XY_coordinate + 70, this.atr_Path_Points[i].atr_Y_XY_coordinate);
                            }
                            else
                            {
                                Graphics.FromImage(my_map).DrawString
                                    (" : " + (i == 0 ? 0 : this.atr_all_path_lenght_in_meters) + " M", atr_myFont, Brushes.Black,
                                this.atr_Path_Points[i].atr_X_XY_coordinate + 20, this.atr_Path_Points[i].atr_Y_XY_coordinate);
                            }
                        }
                    }
                    else
                        Graphics.FromImage(my_map).DrawString("X", atr_myFont, Brushes.Black, this.atr_Path_Points[i].atr_X_XY_coordinate, this.atr_Path_Points[i].atr_Y_XY_coordinate);

                }

                Graphics.FromImage(my_map).DrawLines(Pens.Green, this.m_Get_XY_Points_Array());

                List<Point[]> Green_Region_as_XY_Array = this.m_Get_Green_Region_as_XY_Array(10);
                Graphics.FromImage(my_map).DrawLines(atr_my_pen, Green_Region_as_XY_Array[0]);
                Graphics.FromImage(my_map).DrawLines(atr_my_pen, Green_Region_as_XY_Array[1]);

                for (int i = 0; i < Green_Region_as_XY_Array[0].Length; i++)
                {
                    Graphics.FromImage(my_map).DrawEllipse(Pens.Blue, Green_Region_as_XY_Array[0][i].X - atr_Circle, Green_Region_as_XY_Array[0][i].Y - atr_Circle, 2 * atr_Circle, 2 * atr_Circle);
                    Graphics.FromImage(my_map).DrawEllipse(Pens.Blue, Green_Region_as_XY_Array[1][i].X - atr_Circle, Green_Region_as_XY_Array[1][i].Y - atr_Circle, 2 * atr_Circle, 2 * atr_Circle);
                }

                atr_Map_Image = my_map;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private c_my_Point m_Convert_GPS_Coordinate_to_XY_Coordinate(double Latitude, double Longitude)
        {
            c_my_Point XY = new c_my_Point();
            XY.atr_Latitude_GPS_coordinate = Latitude;
            XY.atr_Longitude_GPS_coordinate = Longitude;
            XY.atr_X_XY_coordinate = (int)((Longitude - atr_Path_Points[0].atr_Longitude_GPS_coordinate) * atr_Zoom_value) - atr_X_Axis_Tranlation;
            XY.atr_Y_XY_coordinate = (int)((atr_Path_Points[0].atr_Latitude_GPS_coordinate - Latitude) * atr_Zoom_value) - atr_Y_Axis_Tranlation;
            return XY;
        }

        private c_my_Point m_Convert_XY_Coordinate_to_GPS_Coordinate(int X, int Y)
        {
            c_my_Point GPS = new c_my_Point();
            GPS.atr_X_XY_coordinate = X;
            GPS.atr_Y_XY_coordinate = Y;
            GPS.atr_Longitude_GPS_coordinate = ((X + atr_X_Axis_Tranlation) / atr_Zoom_value) + atr_Path_Points[0].atr_Longitude_GPS_coordinate;
            GPS.atr_Latitude_GPS_coordinate = -(((Y + atr_Y_Axis_Tranlation) / atr_Zoom_value) - atr_Path_Points[0].atr_Latitude_GPS_coordinate);
            return GPS;
        }

        private Point[] m_Get_XY_Points_Array()
        {
            Point[] current_points = new Point[atr_Path_Points.Count];
            for (int i = 0; i < atr_Path_Points.Count; i++)
            {
                current_points[i] = new Point(atr_Path_Points[i].atr_X_XY_coordinate, atr_Path_Points[i].atr_Y_XY_coordinate);
            }
            return current_points;
        }

        private double m_ToRadians(double x)
        {
            return (x * Math.PI) / 180;
        }

        private double m_Get_Distance_in_Meters(c_my_Point p1, c_my_Point p2)
        {
            m_Convert_Degrees_Minutes_TO_Degrees(p1);
            m_Convert_Degrees_Minutes_TO_Degrees(p2);

            double lat1 = p1.atr_Latitude_GPS_coordinate;
            double lng1 = p1.atr_Longitude_GPS_coordinate;
            double lat2 = p2.atr_Latitude_GPS_coordinate;
            double lng2 = p2.atr_Longitude_GPS_coordinate;

            double earthRadius = 3958.75;
            double dLat = m_ToRadians(lat2 - lat1);
            double dLng = m_ToRadians(lng2 - lng1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(m_ToRadians(lat1)) * Math.Cos(m_ToRadians(lat2)) *
                        Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double dist = earthRadius * c;
            double meterConversion = 1609.00;
            double inmeters = dist * meterConversion;
            return Math.Round(inmeters, 2);
        }

        private int m_Convert_Distance_in_Meter_to_Distance_in_GPS_approximately(int meters)
        {
            int dis_in_GPS = 0;
            c_my_Point p1 = new c_my_Point(0, 0, 0, 0);
            c_my_Point p2 = new c_my_Point(0, 0, 0, 0);
            while (m_Get_Distance_in_Meters(p1, p2.m_My_Clone()) < meters)
            {
                p2.atr_Latitude_GPS_coordinate += 1;
            }
            dis_in_GPS = (int)p2.atr_Latitude_GPS_coordinate;
            return dis_in_GPS;
        }

        private void m_Convert_Degrees_Minutes_TO_Degrees(c_my_Point p)
        {
            string lats = p.atr_Latitude_GPS_coordinate.ToString();
            lats = lats.PadLeft(7, '0');
            lats = lats.Insert(2, ".");
            string longs = p.atr_Longitude_GPS_coordinate.ToString();
            longs = longs.PadLeft(7, '0');
            longs = longs.Insert(2, ".");

            p.atr_Latitude_GPS_coordinate = Convert.ToDouble(lats.Split('.')[0] + "." + (Math.Round(Convert.ToDouble(lats.Split('.')[1]) / 0.6)).ToString().PadLeft(5, '0'));
            p.atr_Longitude_GPS_coordinate = Convert.ToDouble(longs.Split('.')[0] + "." + (Math.Round(Convert.ToDouble(longs.Split('.')[1]) / 0.6)).ToString().PadLeft(5, '0'));
        }

        private List<Point[]> m_Get_Green_Region_as_XY_Array(int Circle_Radius_in_Meters)
        {
            List<c_my_Point>[] temp = atr_Green_Border;
            List<Point>[] all_points = new List<Point>[2];
            Point[] Board1 = new Point[temp[0].Count];
            Point[] Board2 = new Point[temp[0].Count];
            for (int i = 0; i < temp[0].Count; i++)
            {
                Board1[i] = new Point(temp[0][i].atr_X_XY_coordinate, temp[0][i].atr_Y_XY_coordinate);
                Board2[i] = new Point(temp[1][i].atr_X_XY_coordinate, temp[1][i].atr_Y_XY_coordinate);
            }
            List<Point[]> result = new List<Point[]>();
            result.Add(Board1);
            result.Add(Board2);
            return result;
        }

        enum c_Direction { Forward, Right, Left };
        private string m_Get_Direction_and_Degree(Point p1, Point p2, PointF p3)
        {
            {
                //take care with this
                int x1, x2, y1, y2;
                float x3, y3;
                double d1, d2, d3, Cos_alpha;
                //P1
                x1 = p1.X;
                y1 = p1.Y;
                //P2
                x2 = p2.X;
                y2 = p2.Y;
                //P3
                x3 = p3.X;
                y3 = p3.Y;

                //Check that p1 & p2 & p3 are not equal

                d1 = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
                d2 = Math.Sqrt(Math.Pow(y3 - y2, 2) + Math.Pow(x3 - x2, 2));
                d3 = Math.Sqrt(Math.Pow(y3 - y1, 2) + Math.Pow(x3 - x1, 2));
                //Shall Formula
                Cos_alpha = (-Math.Pow(d3, 2) + Math.Pow(d1, 2) + Math.Pow(d2, 2)) /
                    (2 * d1 * d2);
                //Cos_alpha in [-1,1]  Cos(180)= -1 Cos(90)= 0   Cos(0) = 1
                if (Cos_alpha > 1)
                    Cos_alpha = 1;
                else if (Cos_alpha < -1)
                    Cos_alpha = -1;
                double alpha_radian = Math.Acos(Cos_alpha);
                double alpha_degree = Math.Round((alpha_radian * 180) / Math.PI);//convert to degree
                if (Cos_alpha == -1)//[-1 , - 0.7]
                {
                    return (c_Direction.Forward.ToString() + ":" + alpha_degree.ToString());
                }
                else //either Left or Right
                {
                    //translate to (0,0)

                    int X1, X2, X3, Y1, Y2, Y3;
                    int mx = 0, my = 0;
                    X1 = mx;
                    Y1 = my;

                    my += x2 - x1;
                    mx += y2 - y1;

                    X2 = mx;
                    Y2 = my;

                    my += (int)(x3 - x2);
                    mx += (int)(y3 - y2);

                    X3 = mx;
                    Y3 = my;

                    x1 = X1; x2 = X2; x3 = X3;
                    y1 = Y1; y2 = Y2; y3 = Y3;


                    // where is the point 2? it is in part (1 | 2 | 3 | 4)
                    if (x2 >= 0)
                    {
                        // in part 1 | 4
                        if (x2 == 0)
                        {
                            if (x3 > 0)
                            {
                                if (y2 > 0)
                                    return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                                else
                                    return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                            }
                            else
                            {
                                if (y2 > 0)
                                    return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                                else
                                    return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            }
                        }
                        else
                        {
                            float slope = (float)y2 / (float)x2;
                            //in part 1 | 4
                            if (slope * x3 > y3)
                                return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            else
                                return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        }
                    }
                    else
                    {
                        // in part 2 | 3
                        float slope = (float)y2 / (float)x2;
                        if (slope * x3 > y3)
                            return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        else
                            return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                    }
                }
            }
        }

        private Image m_cropImage(Image img, Rectangle cropArea)
        {
            if (cropArea.X < 0)
                cropArea.X = 0;
            if (cropArea.Y < 0)
                cropArea.Y = 0;
            Size new_size = cropArea.Size;
            if (cropArea.X + cropArea.Width > img.Width)
                new_size.Width = cropArea.X + cropArea.Width;
            if (cropArea.Y + cropArea.Height > img.Height)
                new_size.Height = cropArea.Y + cropArea.Height;
            Bitmap new_bitmap = new Bitmap(new_size.Width + 1, new_size.Height + 1);
            Graphics.FromImage(new_bitmap).DrawImage(img, 0, 0);

            if (cropArea.Width != 0 && cropArea.Height != 0)
            {
                return new_bitmap.Clone(cropArea,
                img.PixelFormat);
            }
            return img;
        }

        private Image m_ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            int destWidth = (int)(sourceWidth * nPercentW);
            int destHeight = (int)(sourceHeight * nPercentH);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }
    }
    class c_my_Line
    {
        // Y = Slope * X + C
        public PointF p1, p2;

        public c_my_Line m_Clone()
        {
            return new c_my_Line(p1, p2);
        }

        public double Slope
        {
            get { return (double)(p2.Y - p1.Y) / (double)(p2.X - p1.X); }
        }

        public double C
        {
            get { return p2.Y - (Slope * p2.X); }
        }

        public c_my_Line(PointF _p1, PointF _p2)
        {
            p1.X = _p1.X;
            p2.X = _p2.X;

            p1.Y = _p1.Y;
            p2.Y = _p2.Y;
        }

        public bool m_test_line()
        {
            if (Math.Abs(p1.X - p2.X) >= 1 &&
                Math.Abs(p1.Y - p2.Y) >= 1)
                return true;
            return false;
        }

        public Point m_Intersect(c_my_Line Line1)
        {
            if (Math.Round(Math.Abs(Line1.Slope - this.Slope), 1) > 0)
            {
                double temp = (((-Line1.Slope * this.C) / this.Slope) + Line1.C);
                double temp2 = (1 - (Line1.Slope / this.Slope));
                double y = temp / temp2;
                double x = (y - this.C) / this.Slope;
                return new Point((int)x, (int)y);
            }
            else
                return new Point((int)Line1.p1.X, (int)Line1.p1.Y);
        }
    }

    class c_my_Point
    {
        public int atr_X_XY_coordinate;
        public int atr_Y_XY_coordinate;

        public double atr_Latitude_GPS_coordinate;
        public double atr_Longitude_GPS_coordinate;

        public c_my_Point m_My_Clone()
        {
            return new c_my_Point(this.atr_X_XY_coordinate, this.atr_Y_XY_coordinate, this.atr_Latitude_GPS_coordinate, this.atr_Longitude_GPS_coordinate);
        }

        public c_my_Point()
        {

        }

        public Point m_Get_default_Point()
        {
            return new Point(atr_X_XY_coordinate, atr_Y_XY_coordinate);
        }

        public c_my_Point(int _X_XY_coordinate, int _Y_XY_coordinate, double _Latitude_GPS_coordinate, double _Longitude_GPS_coordinate)
        {
            atr_X_XY_coordinate = _X_XY_coordinate;
            atr_Y_XY_coordinate = _Y_XY_coordinate;
            atr_Latitude_GPS_coordinate = _Latitude_GPS_coordinate;
            atr_Longitude_GPS_coordinate = _Longitude_GPS_coordinate;
        }

        public c_my_Point(double _Latitude_GPS_coordinate, double _Longitude_GPS_coordinate)
        {
            atr_Latitude_GPS_coordinate = _Latitude_GPS_coordinate;
            atr_Longitude_GPS_coordinate = _Longitude_GPS_coordinate;
        }

        public c_my_Point(int _X_XY_coordinate, int _Y_XY_coordinate)
        {
            atr_X_XY_coordinate = _X_XY_coordinate;
            atr_Y_XY_coordinate = _Y_XY_coordinate;
        }

        public c_my_Point(Point p)
        {
            atr_X_XY_coordinate = p.X;
            atr_Y_XY_coordinate = p.Y;
        }
    }

    class c_my_Region
    {
        public c_my_Point[] atr_Points = new c_my_Point[4];
        // 0 --> 1
        // 3 <-- 2
        public c_my_Point atr_Sound_Beacon = new c_my_Point();

        public Point[] m_Get_defualt_Points()
        {
            return new Point[4]{atr_Points[0].m_Get_default_Point(),
                atr_Points[1].m_Get_default_Point(),
                atr_Points[2].m_Get_default_Point(),
                atr_Points[3].m_Get_default_Point()};
        }

        public c_my_Region(c_my_Point[] _Points, c_my_Point _Sound_Beacon)
        {
            atr_Points = _Points;
            atr_Sound_Beacon = _Sound_Beacon;
        }

        public int m_distance_from_In_XY_coordinate(c_my_Point point)
        {
            return (int)Math.Sqrt(Math.Pow(atr_Sound_Beacon.atr_X_XY_coordinate - point.atr_X_XY_coordinate, 2) +
                Math.Pow(atr_Sound_Beacon.atr_Y_XY_coordinate - point.atr_Y_XY_coordinate, 2));
        }

        public bool m_Is_In(c_my_Point point)
        {
            string direction1 = m_Get_Direction_and_Degree
                (atr_Points[0].m_Get_default_Point(),
                atr_Points[1].m_Get_default_Point(),
                point.m_Get_default_Point());

            string direction2 = m_Get_Direction_and_Degree
                (atr_Points[3].m_Get_default_Point(),
                atr_Points[2].m_Get_default_Point(),
                point.m_Get_default_Point());

            string direction3 = m_Get_Direction_and_Degree
                (atr_Points[3].m_Get_default_Point(),
                atr_Points[0].m_Get_default_Point(),
                point.m_Get_default_Point());

            string direction4 = m_Get_Direction_and_Degree
                (atr_Points[2].m_Get_default_Point(),
                atr_Points[1].m_Get_default_Point(),
                point.m_Get_default_Point());

            if (direction1[0] == 'R' && direction2[0] == 'L')
            {
                if
                    ((direction3[0] == 'R' && direction4[0] == 'L')
                    || (direction3[0] == 'L' && direction4[0] == 'R'))
                {
                    return true;
                }
            }
            else if (direction1[0] == 'L' && direction2[0] == 'R')
            {
                if
                    ((direction3[0] == 'R' && direction4[0] == 'L')
                    || (direction3[0] == 'L' && direction4[0] == 'R'))
                {
                    return true;
                }
            }
            return false;
        }

        enum c_Direction { Forward, Right, Left };
        private string m_Get_Direction_and_Degree(Point p1, Point p2, Point p3)
        {
            {
                //take care with this
                int x1, x2, x3, y1, y2, y3;
                double d1, d2, d3, Cos_alpha;
                //P1
                x1 = p1.X;
                y1 = p1.Y;
                //P2
                x2 = p2.X;
                y2 = p2.Y;
                //P3
                x3 = p3.X;
                y3 = p3.Y;

                //Check that p1 & p2 & p3 are not equal

                d1 = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
                d2 = Math.Sqrt(Math.Pow(y3 - y2, 2) + Math.Pow(x3 - x2, 2));
                d3 = Math.Sqrt(Math.Pow(y3 - y1, 2) + Math.Pow(x3 - x1, 2));
                //Shall Formula
                Cos_alpha = (-Math.Pow(d3, 2) + Math.Pow(d1, 2) + Math.Pow(d2, 2)) /
                    (2 * d1 * d2);
                //Cos_alpha in [-1,1]  Cos(180)= -1 Cos(90)= 0   Cos(0) = 1
                if (Cos_alpha > 1)
                    Cos_alpha = 1;
                else if (Cos_alpha < -1)
                    Cos_alpha = -1;
                double alpha_radian = Math.Acos(Cos_alpha);
                double alpha_degree = Math.Round((alpha_radian * 180) / Math.PI);//convert to degree
                if (Cos_alpha == -1)//[-1 , - 0.7]
                {
                    return (c_Direction.Forward.ToString() + ":" + alpha_degree.ToString());
                }
                else //either Left or Right
                {
                    //translate to (0,0)

                    int X1, X2, X3, Y1, Y2, Y3;
                    int mx = 0, my = 0;
                    X1 = mx;
                    Y1 = my;

                    my += x2 - x1;
                    mx += y2 - y1;

                    X2 = mx;
                    Y2 = my;

                    my += x3 - x2;
                    mx += y3 - y2;

                    X3 = mx;
                    Y3 = my;

                    x1 = X1; x2 = X2; x3 = X3;
                    y1 = Y1; y2 = Y2; y3 = Y3;


                    // where is the point 2? it is in part (1 | 2 | 3 | 4)
                    if (x2 >= 0)
                    {
                        // in part 1 | 4
                        if (x2 == 0)
                        {
                            if (x3 > 0)
                            {
                                if (y2 > 0)
                                    return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                                else
                                    return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                            }
                            else
                            {
                                if (y2 > 0)
                                    return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                                else
                                    return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            }
                        }
                        else
                        {
                            float slope = (float)y2 / (float)x2;
                            //in part 1 | 4
                            if (slope * x3 > y3)
                                return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            else
                                return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        }
                    }
                    else
                    {
                        // in part 2 | 3
                        float slope = (float)y2 / (float)x2;
                        if (slope * x3 > y3)
                            return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        else
                            return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                    }
                }
            }
        }
    }
}