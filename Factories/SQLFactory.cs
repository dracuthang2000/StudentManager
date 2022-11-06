using StudentManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Factories
{
    public static class SQLFactory
    {
        static string thisServerName;
        static string login;
        static string password;
        static string username;
        static string fullName;
        static string group;

        static string loginRemote = "HTKN";
        static string passwordRemote = "123456";
        static string loginStudent = "SV";
        static string passwordStudent = "123456";
        static string currentServer { get; set; }
        public static SqlConnection GetConnection() {
            string conmStr;
            if (currentServer == thisServerName)
            {
                conmStr =
                String.Format("Data Source={0} ;Database=QLDSSV_TC ;Persist Security Info=True;User ID={1}; password={2}",
                                thisServerName, login, password);
            }
            else
            {
                // user remote serversql
                conmStr =
                String.Format("Data Source={0} ;Database=QLDSSV_TC ;Persist Security Info=True;User ID={1}; password={2}",
                                currentServer, loginRemote, passwordRemote);
            }
             var conn = new SqlConnection(conmStr);

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                return conn;
            }
            throw new Exception("Cannot open connection");
        }
        public static void SetServer(string serverName, bool notifyChange = true)
        {
            thisServerName = serverName;
        }

        public static void SetCurrentServer(string serverName, bool notifyChange = true)
        {
            currentServer = serverName;
        }

        public static void setUser(string alogin, string apassword)
        {
            login = alogin;
            password = apassword;
        }
    }
}