using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
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
                Common.ExportError(ex, "CheckConnectServer");
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
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "ExecuteQuery");
            }
            return dataTable;
        }

        public DataTable ExecuteQuery(List<string> lstQuery)
        {
            DataTable dataTable = new DataTable();
            try
            {
                this.CheckConnectServer();
                for (int i = 0; i < lstQuery.Count; i++)
                {
                    string commandText = lstQuery[i];
                    SQLiteCommand cmd = new SQLiteCommand(commandText, this.connection);
                    SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(cmd);
                    sqliteDataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "ExecuteQuery");
            }
            return dataTable;
        }

        public int ExecuteNonQuery(List<string> lstQuery)
        {
            int result = 0;
            try
            {
                this.CheckConnectServer();
                for (int i = 0; i < lstQuery.Count; i++)
                {
                    string commandText = lstQuery[i];
                    SQLiteCommand sqliteCommand = new SQLiteCommand(commandText, this.connection);
                    result = sqliteCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "ExecuteNonQuery");
            }
            return result;
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
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "ExecuteNonQuery: " + query);
            }
            return result;
        }

        public int ExecuteScalar(string query)
        {
            int result = 0;
            try
            {
                this.CheckConnectServer();
                SQLiteCommand sqliteCommand = new SQLiteCommand(query, this.connection);
                long num = (long)sqliteCommand.ExecuteScalar();
                result = (int)num;
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "ExecuteScalar: " + query);
            }
            return result;
        }

        private static Connector instance;

        private string connectionSTR = "Data Source=database/db_fbtool.sqlite;Version=3;";

        private SQLiteConnection connection = null;
    }
}
