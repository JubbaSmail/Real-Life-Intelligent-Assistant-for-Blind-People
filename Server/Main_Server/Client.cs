using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Main_Server
{
    class Client
    {
        public int C_ID;
        public string C_FirstName;
        public string C_LastName;
        public string C_Nickname;
        public string C_Password;
        public DateTime C_BirthDate;
        public int C_Age;
        public bool C_Gender;
        public string C_Nationality;
        public string C_Address;
        public string C_Email;
        public string C_Phone;
        public string C_Mobile;
        public string C_School;
        public string C_University;

        private SqlConnection conn;
        private SqlCommand command;
        private SqlDataReader sdr;
        private List<Client> resClients;
        private List<int> resC_IDs;
        public static string connstring = "Data Source=IMLJH-PC\\SQLEXPRESS;AttachDbFilename=\"C:\\Program Files\\Microsoft SQL Server\\MSSQL.1\\MSSQL\\Data\\SeniorDB.mdf\";Integrated Security=True";
        //public static string connstring = "Data Source=MASTER-PC;Initial Catalog=SeniorDB;Integrated Security=True";

        public bool AddClient(string FirstName, string LastName, string Nickname, string Password,
            DateTime BirthDate, int Age, bool Gender, string Nationality, string Address, string Email,
            string Phone, string Mobile, string School, string University)
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.AddClient", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_FirstName", FirstName));
                command.Parameters.Add(new SqlParameter("@C_LastName", LastName));
                command.Parameters.Add(new SqlParameter("@C_Nickname", Nickname));
                command.Parameters.Add(new SqlParameter("@C_Password", Password));
                command.Parameters.Add(new SqlParameter("@C_BirthDate", BirthDate));
                command.Parameters.Add(new SqlParameter("@C_Age", Age));
                command.Parameters.Add(new SqlParameter("@C_Gender", Gender));
                command.Parameters.Add(new SqlParameter("@C_Nationality", Nationality));
                command.Parameters.Add(new SqlParameter("@C_Address", Address));
                command.Parameters.Add(new SqlParameter("@C_Email", Email));
                command.Parameters.Add(new SqlParameter("@C_Phone", Phone));
                command.Parameters.Add(new SqlParameter("@C_Mobile", Mobile));
                command.Parameters.Add(new SqlParameter("@C_School", School));
                command.Parameters.Add(new SqlParameter("@C_University", University));
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public bool UpdateClient(int ID, string FirstName, string LastName, string Nickname, string Password,
            DateTime BirthDate, int Age, bool Gender, string Nationality, string Address, string Email,
            string Phone, string Mobile, string School, string University)
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.UpdateClient", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_ID", ID));
                command.Parameters.Add(new SqlParameter("@C_FirstName", FirstName));
                command.Parameters.Add(new SqlParameter("@C_LastName", LastName));
                command.Parameters.Add(new SqlParameter("@C_Nickname", Nickname));
                command.Parameters.Add(new SqlParameter("@C_Password", Password));
                command.Parameters.Add(new SqlParameter("@C_BirthDate", BirthDate));
                command.Parameters.Add(new SqlParameter("@C_Age", Age));
                command.Parameters.Add(new SqlParameter("@C_Gender", Gender));
                command.Parameters.Add(new SqlParameter("@C_Nationality", Nationality));
                command.Parameters.Add(new SqlParameter("@C_Address", Address));
                command.Parameters.Add(new SqlParameter("@C_Email", Email));
                command.Parameters.Add(new SqlParameter("@C_Phone", Phone));
                command.Parameters.Add(new SqlParameter("@C_Mobile", Mobile));
                command.Parameters.Add(new SqlParameter("@C_School", School));
                command.Parameters.Add(new SqlParameter("@C_University", University));
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public bool DeleteClient(int ID)
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.DeleteClient", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_ID", ID));
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public List<Client> GetAllClients()
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.GetAllClients", conn);
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
                resClients = new List<Client>();
                while (sdr.Read())
                {
                    resClients[pointer].C_ID = int.Parse(sdr[pointer * 15].ToString());
                    resClients[pointer].C_FirstName = sdr[pointer * 15 + 1].ToString();
                    resClients[pointer].C_LastName = sdr[pointer * 15 + 2].ToString();
                    resClients[pointer].C_Nickname = sdr[pointer * 15 + 3].ToString();
                    resClients[pointer].C_Password = sdr[pointer * 15 + 4].ToString();
                    resClients[pointer].C_BirthDate = DateTime.Parse(sdr[pointer * 15 + 5].ToString());
                    resClients[pointer].C_Age = int.Parse(sdr[pointer * 15 + 6].ToString());
                    resClients[pointer].C_Gender = bool.Parse(sdr[pointer * 15 + 7].ToString());
                    resClients[pointer].C_Nationality = sdr[pointer * 15 + 8].ToString();
                    resClients[pointer].C_Address = sdr[pointer * 15 + 9].ToString();
                    resClients[pointer].C_Email = sdr[pointer * 15 + 10].ToString();
                    resClients[pointer].C_Phone = sdr[pointer * 15 + 11].ToString();
                    resClients[pointer].C_Mobile = sdr[pointer * 15 + 12].ToString();
                    resClients[pointer].C_School = sdr[pointer * 15 + 13].ToString();
                    resClients[pointer].C_University = sdr[pointer * 15 + 14].ToString();
                    pointer++;
                }
                conn.Close();
                sdr.Close();
                return resClients;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public Client GetClientByID(int ID)
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.GetClientByID", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_ID", ID));
                sdr = command.ExecuteReader();
                resClients = new List<Client>();
                while (sdr.Read())
                {
                    resClients[0].C_ID = int.Parse(sdr[0].ToString());
                    resClients[0].C_FirstName = sdr[1].ToString();
                    resClients[0].C_LastName = sdr[2].ToString();
                    resClients[0].C_Nickname = sdr[3].ToString();
                    resClients[0].C_Password = sdr[4].ToString();
                    resClients[0].C_BirthDate = DateTime.Parse(sdr[5].ToString());
                    resClients[0].C_Age = int.Parse(sdr[6].ToString());
                    resClients[0].C_Gender = bool.Parse(sdr[7].ToString());
                    resClients[0].C_Nationality = sdr[8].ToString();
                    resClients[0].C_Address = sdr[9].ToString();
                    resClients[0].C_Email = sdr[10].ToString();
                    resClients[0].C_Phone = sdr[11].ToString();
                    resClients[0].C_Mobile = sdr[12].ToString();
                    resClients[0].C_School = sdr[13].ToString();
                    resClients[0].C_University = sdr[14].ToString();
                }
                conn.Close();
                sdr.Close();
                return resClients[0];
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public Client GetClientByName(string Name)
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.GetClientByName", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_Name", Name));
                sdr = command.ExecuteReader();
                resClients = new List<Client>();
                while (sdr.Read())
                {
                    resClients[0].C_ID = int.Parse(sdr[0].ToString());
                    resClients[0].C_FirstName = sdr[1].ToString();
                    resClients[0].C_LastName = sdr[2].ToString();
                    resClients[0].C_Nickname = sdr[3].ToString();
                    resClients[0].C_Password = sdr[4].ToString();
                    resClients[0].C_BirthDate = DateTime.Parse(sdr[5].ToString());
                    resClients[0].C_Age = int.Parse(sdr[6].ToString());
                    resClients[0].C_Gender = bool.Parse(sdr[7].ToString());
                    resClients[0].C_Nationality = sdr[8].ToString();
                    resClients[0].C_Address = sdr[9].ToString();
                    resClients[0].C_Email = sdr[10].ToString();
                    resClients[0].C_Phone = sdr[11].ToString();
                    resClients[0].C_Mobile = sdr[12].ToString();
                    resClients[0].C_School = sdr[13].ToString();
                    resClients[0].C_University = sdr[14].ToString();
                }
                conn.Close();
                sdr.Close();
                return resClients[0];
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public List<int> GetClientsPath(int P_ID)
        {

            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("dbo.GetClientsPath", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return null;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@P_ID", P_ID));
                sdr = command.ExecuteReader();
                resC_IDs = new List<int>();
                int pointer = 0;
                while (sdr.Read())
                {
                    resC_IDs[pointer] = int.Parse(sdr[pointer].ToString());
                    pointer++;
                }
                conn.Close();
                sdr.Close();
                return resC_IDs;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public bool ClientLogIn(string Nickname, string Password)
        {
            try
            {
                conn = new SqlConnection(connstring);
                command = new SqlCommand("ClientLogIn", conn);
                command.Connection.Open();
            }
            catch (System.Exception ex)
            {
                return false;
            }

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@C_NickName", Nickname));
                command.Parameters.Add(new SqlParameter("@C_Password", Password));
                sdr = command.ExecuteReader();
                resClients = new List<Client>();
                if (sdr.HasRows)
                {
                    conn.Close();
                    sdr.Close();
                    return true;
                }
                conn.Close();
                sdr.Close();
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}