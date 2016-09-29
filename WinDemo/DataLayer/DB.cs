using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace DataLayer
{
    public class ConnectionStatistics
    {
        public IDictionary OriginalStats { get; set; }
        public long ExecutionTime { get; set; }
        public long BytesReceived { get; set; }

        public ConnectionStatistics(IDictionary stats)
        {
            OriginalStats = stats;
            if (stats.Contains("ExecutionTime")) ExecutionTime = int.Parse(stats["ExecutionTime"].ToString());
            if (stats.Contains("BytesReceived")) BytesReceived = int.Parse(stats["BytesReceived"].ToString());
        }
    }
    public class DB
    {
        public static ConnectionStatistics LastStatistics { get; set; }
        public static bool EnableStatistics { get; set; }
        public static string ConnectionString
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AWConnection"].ConnectionString;
                SqlConnectionStringBuilder stb = new SqlConnectionStringBuilder(connectionString);
                stb.ConnectTimeout = ConnectionTimeout > 0 ? ConnectionTimeout : stb.ConnectTimeout;
                stb.ApplicationName = ApplicationName ?? stb.ApplicationName;
                return stb.ToString();
            }
        }


        public static SqlConnection GetSqlConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            conn.StatisticsEnabled = EnableStatistics;
            conn.StateChange += conn_StateChange;
            return conn;
        }

        private static void conn_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                if (((SqlConnection)sender).StatisticsEnabled)
                {
                    LastStatistics = new ConnectionStatistics(((SqlConnection)sender).RetrieveStatistics());
                }
            }
        }

        public static int ConnectionTimeout { get; set; }
        public static string ApplicationName { get; set; }
    }
}
