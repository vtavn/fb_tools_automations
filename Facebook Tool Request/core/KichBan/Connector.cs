using System;
using System.Data;
using System.Data.SQLite;

namespace Facebook_Tool_Request.core.KichBan
{
    public class Connector
    {
        public static Connector Instance
        {
            get
            {
                bool flag = Connector.instance == null;
                if (flag)
                {
                    Connector.instance = new Connector();
                }
                return Connector.instance;
            }
            private set
            {
                Connector.instance = value;
            }
        }

        private Connector()
        {
        }

        private void CheckConnectServer()
        {
            try
            {
                bool flag = this.connection == null;
                if (flag)
                {
                    this.connection = new SQLiteConnection(this.connectionSTR);
                }
                bool flag2 = this.connection.State == ConnectionState.Closed;
                if (flag2)
                {
                    this.connection.Open();
                }
            }
            catch (Exception ex)
            {
                Facebook_Tool_Request.Helpers.Common.ExportError(ex, "CheckConnectServer");
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                this.CheckConnectServer();
                SQLiteCommand cmd = new SQLiteCommand(query, this.connection);
                SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(cmd);
                sqliteDataAdapter.Fill(dataTable);
            }
            catch
            {
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string query)
        {
            int result = 0;
            try
            {
                this.CheckConnectServer();
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, this.connection);
                result = sqliteCommand.ExecuteNonQuery();
            }
            catch
            {
            }
            return result;
        }

        public int ExecuteScalar(string query)
        {
            int result = -1;
            try
            {
                this.CheckConnectServer();
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, this.connection);
                long num = (long)sqliteCommand.ExecuteScalar();
                result = (int)num;
            }
            catch
            {
            }
            return result;
        }

        private static Connector instance;

        private string connectionSTR = "Data Source=database\\kichban.sqlite;Version=3;";

        private SQLiteConnection connection = null;
    }
}