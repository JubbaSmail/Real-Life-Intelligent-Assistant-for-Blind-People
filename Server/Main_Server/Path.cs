using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Xml;
using System.Drawing;

namespace Main_Server
{
    class Path
    {
        public int P_ID;
        public string P_Name;
        public string P_XML;
        public Image P_Image;

        private SqlConnection conn;
        private SqlCommand command;
        private SqlDataReader sdr;
        private List<Path> resPaths;
        private Path resPath;
        private List<int> resP_IDs;
        public bool AddPath(string Name, string XML)
        {
            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.AddPath", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@P_Name", Name));
                command.Parameters.Add(new SqlParameter("@P_XML", XML));
                //command.Parameters.Add(new SqlParameter("@P_Image", null));
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public bool UpdatePath(int ID, string Name, string XML, Image imag)
        {
            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.UpdatePath", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@P_ID", ID));
                command.Parameters.Add(new SqlParameter("@P_Name", Name));
                command.Parameters.Add(new SqlParameter("@P_XML", XML));
                command.Parameters.Add(new SqlParameter("@P_Image", imag));
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public bool DeletePath(int ID)
        {
            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.DeletePath", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@P_ID", ID));
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public List<Path> GetAllPaths()
        {
            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.GetAllPaths", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                sdr = command.ExecuteReader();
                int pointer = 0;
                resPaths = new List<Path>();
                while (sdr.Read())
                {
                    resPaths[pointer].P_ID = int.Parse(sdr[pointer * 3].ToString());
                    resPaths[pointer].P_Name = sdr[pointer * 3 + 1].ToString();
                    resPaths[pointer].P_XML = sdr[pointer * 3 + 2].ToString();
                    resPaths[pointer].P_Image = (Image)sdr[pointer * 3 + 3];
                    pointer++;
                }
                conn.Close();
                sdr.Close();
                return resPaths;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public Path GetPathByID(int ID)
        {
            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.GetPathByID", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@P_ID", ID));
                sdr = command.ExecuteReader();
                resPaths = new List<Path>();
                while (sdr.Read())
                {
                    resPaths[0].P_ID = int.Parse(sdr[0].ToString());
                    resPaths[0].P_Name = sdr[1].ToString();
                    resPaths[0].P_XML = sdr[2].ToString();
                    resPaths[0].P_Image = (Image)sdr[3];
                }
                conn.Close();
                sdr.Close();
                return resPaths[0];
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public Path GetPathByName(string Name)
        {
            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.GetPathByName", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@P_Name", Name));
                sdr = command.ExecuteReader();
                resPath = new Path();
                while (sdr.Read())
                {
                    resPath.P_ID = int.Parse(sdr[0].ToString());
                    resPath.P_Name = sdr[1].ToString();
                    resPath.P_XML = sdr[2].ToString();
                    //resPath.P_Image = (Image)sdr[3];

                    conn.Close();
                    sdr.Close();
                    return resPath;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
            return null;
        }

        public List<int> GetPathsClient(int C_ID)
        {

            try
            {
                conn = new SqlConnection(Client.connstring);
                command = new SqlCommand("dbo.GetPathsClient", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_ID", C_ID));
                sdr = command.ExecuteReader();
                resP_IDs = new List<int>();
                int pointer = 0;
                while (sdr.Read())
                {
                    resP_IDs[pointer] = int.Parse(sdr[pointer].ToString());
                    pointer++;
                }
                conn.Close();
                sdr.Close();
                return resP_IDs;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
    }
}