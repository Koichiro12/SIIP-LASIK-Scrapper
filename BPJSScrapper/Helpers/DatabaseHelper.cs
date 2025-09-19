using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BPJSScrapper.Helpers
{
    class DatabaseHelper
    {

        public string server { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string database { get; set; }
        public string lastError { get; set; }
        string connectionStr;
        MySqlConnection conn;
        public DatabaseHelper()
        {
            this.server = "localhost";
            this.user = "root";
            this.password = "";
            this.database = "botdb";
            this.lastError = "";
            this.connectionStr = "server="+this.server+";user="+ this.user + ";password="+this.password+";database="+this.database+";";
        }
        public DatabaseHelper(string server, string user, string password, string database)
        {
            this.server = server;
            this.user = user;
            this.password = password;
            this.database = database;
            this.connectionStr = "server=" + this.server + ";user=" + this.user + ";password=" + this.password + ";database=" + this.database + ";";
        }

        public MySqlConnection GetConnection()
        {
            conn = new MySqlConnection(connectionStr);
            return conn;
        }

        public bool TestConnection()
        {
            try
            {
                conn = new MySqlConnection(connectionStr);
                conn.Open();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                this.lastError = ex.Message;
                return false;
            }
        }

        public string getHWID()
        {
            try
            {
                string hwid = "";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get())
                {
                    hwid = obj["ProcessorId"].ToString();
                    break;
                }
                return hwid;
            }
            catch (Exception ex)
            {
                return "UNKNOWN_HWID";
            }

        }

    }
}
