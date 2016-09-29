using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace DataLayer
{
    public class ApplicationLog
    {
        public static void Add(string comment)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"AddAppLog";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter commentParameter = new SqlParameter("comment", SqlDbType.NText, 100)
                    {
                        Value = comment
                    };

                    cmd.Parameters.Add(commentParameter);
                    int result = cmd.ExecuteNonQuery();
                }
            }
        }
        public static void Add2(string comment)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"AddAppLog2";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter commentParameter = new SqlParameter("comment", SqlDbType.NText, 100)
                    {
                        Value = comment
                    };

                    cmd.Parameters.Add(commentParameter);
                    object result = cmd.ExecuteScalar();
                }
            }
        }
        public static void Add3(string comment)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"AddAppLog3";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter commentParameter = new SqlParameter("comment", SqlDbType.NText, 100)
                    {
                        Value = comment
                    };
                    cmd.Parameters.Add(commentParameter);

                    SqlParameter p2 = new SqlParameter("outid", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(p2);
                    cmd.ExecuteNonQuery();

                    object res = p2.Value;
                }
            }
        }
        public static void Add4(string comment)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"AddAppLog4";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter commentParameter = new SqlParameter("comment", SqlDbType.NText, 100)
                    {
                        Value = comment
                    };
                    cmd.Parameters.Add(commentParameter);

                    SqlParameter p2 = new SqlParameter("outid", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(p2);
                    cmd.ExecuteNonQuery();

                    object res = p2.Value;
                }
            }
        }
        public static void DeleteCommentsForApp(string appName)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DeleteAppLog";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter commentParameter = new SqlParameter("appName", SqlDbType.NText, 100)
                    {
                        Value = appName
                    };
                    cmd.Parameters.Add(commentParameter);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static DataTable GetLog(string appName)
        {
            DataTable table = new DataTable("ApplicationLog");
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ApplicationLog WHERE application_name = @appName", conn);
                cmd.Parameters.Add(new SqlParameter("appName", SqlDbType.NVarChar, 100));
                cmd.Parameters["appName"].Value = appName;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                int res = adapter.Fill(table);
            }
            return table;
        }
        public static DataTable UpdateLogChanges(DataTable tableLog)
        {
            using (SqlConnection conn = DB.GetSqlConnection())
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT * FROM ApplicationLog", conn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
                int res = adapter.Update(tableLog);
            }
            return tableLog;
        }

    }
}