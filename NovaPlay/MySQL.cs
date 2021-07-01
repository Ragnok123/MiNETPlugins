using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using log4net;

namespace NovaPlay
{
    public class MySQL
    {

        public NovaCore novacore;
        public MySqlConnection connection;

        public static ILog Logger = LogManager.GetLogger(typeof(MySQL));

        public MySQL(NovaCore novacore)
        {
            this.novacore = novacore;
        }

        public MySqlConnection Connect()
        {
            string server = "0.0.0.0";
            string database = "database";
            string username = "username";
            string password = "password";
            string connector = "Server=" + server + ";" + "Database=" +
            database + ";" + "Uid=" + username + ";" + "Password=" + password + ";";
            try
            {
                connection = new MySqlConnection(connector);
                connection.Open();
                Logger.Error("[NovaCore] Pripojeno k MySQL databazi");
            } catch (MySqlException exception)
            {
                Logger.Error("[NovaCore] Nepodarilo se pripojit k MySQL databazi");
                Logger.Error("[NovaCore] MySQL error n:" + exception.Number);
                NovaCore.GetServer().StopServer();
            }
            return connection;
        }


        public DataTable Queryy(string query, bool isQuery = true)
        {
            var db = NovaCore.GetInstance().mysql.connection;
            if (isQuery)
            {
                DataTable dt = new DataTable();
                var cmd = db.CreateCommand();
                cmd.CommandText = @query;
                try
                {
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                            return dt;
                        }
                        dr.Close();
                    }
                    return null;
                }
                catch (MySqlException e)
                {
                    Logger.Error("MYSQL Error select:" + e.ToString());
                    return null;
                }

            }
            else
            {
                try
                {
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandText = @query;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException e)
                {
                    Logger.Error("MYSQL Error:" + e.ToString());
                }
                return null;
            }
        }

        public Dictionary<string, string> Query(string sql1)
        {
            DataTable sql = Queryy(sql1);
            if (sql != null)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>(sql.Columns.Count);
                foreach (DataColumn c in sql.Columns)
                {
                    dic.Add(c.ColumnName, "null");
                    DataRow row = sql.Rows[0];
                    dic[c.ColumnName] = row[c.ColumnName].ToString();
                    //dic[c.ColumnName] = sql.Rows[0].Field<object>(c.ColumnName).ToString();
                }
                sql.Dispose();
                return dic.Count > 0 ? dic : null;
            }
            return null;
        }
        


        public void ExecuteQuery(string query)
        {
            var db = NovaCore.GetInstance().mysql.connection;
            var cmd = db.CreateCommand();
            cmd.CommandText = @query;
            cmd.ExecuteNonQuery();
        }



    }
}
