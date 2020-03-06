using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Utilities.Log;

namespace Utilities.DataBaseConnection.MySQL
{
    public class DataHandling
    {
        private Connection Connection { get; set; }
        
        public DataHandling(Connection connection)
        {
            this.Connection = connection;
        }

        public Boolean ConnectionTest()
        {
            Boolean result = false;
            try
            {
                Connection.MySQLConnection.Open();
                result = true;
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("SQLServer.ConnectionTest; {0} {1} >> {2}", Environment.NewLine, ex.Message, ex.StackTrace));
            }
            finally
            {
                Connection.MySQLConnection.Close();
            }
            return result;
        }

        public DataSet ReadQuery(String SQL, List<MySqlParameter> mySQLParameters = null)
        {
            DataSet result = new DataSet();
            try
            {
                Connection.MySQLConnection.Open();
                using (MySqlCommand cmd = new MySqlCommand(SQL, Connection.MySQLConnection))
                {
                    if (mySQLParameters != null) 
                    {
                        cmd.Parameters.AddRange(mySQLParameters.ToArray());
                    }
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    dataAdapter.Fill(result);
                }
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("MySQL.ReadQuery; {0} {1} >> {2} {0} SQL:{3};", Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                Connection.MySQLConnection.Close();
            }
            return result;
        }


    }
}
