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
        public MySqlConnection MySQLConnection { get; private set; }

        public DataHandling(String server, String dataBase, String user, String password)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

            builder.Database = dataBase;
            builder.Server = server;
            builder.UserID = user;
            builder.Password = password;

            this.MySQLConnection = new MySqlConnection(builder.ConnectionString);
        }

        public DataHandling(String server, String dataBase, String user, String password, uint port)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

            builder.Database = dataBase;
            builder.Server = server;
            builder.UserID = user;
            builder.Password = password;
            builder.Port = port;

            this.MySQLConnection = new MySqlConnection(builder.ConnectionString);
        }

        /// <summary>
        ///     Connection test method;
        ///     Metodo para o teste de conexão;
        /// </summary>
        /// <returns>
        ///     If it fails, the method logs the error in the system log with the application name;
        ///     Caso falhe, o metodo registra o erro em log de sistema com o nome da aplicação;
        ///     (Boolean)
        /// </returns>
        public Boolean ConnectionTest()
        {
            Boolean result = false;
            try
            {
                MySQLConnection.Open();
                result = true;
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("SQLServer.ConnectionTest; {0} {1} >> {2}", Environment.NewLine, ex.Message, ex.StackTrace));
            }
            finally
            {
                MySQLConnection.Close();
            }
            return result;
        }

        /// <summary>
        ///     Method for executing SQL commands with data return;
        ///     Metodo para execução de comandos SQL com retorno de dados;
        /// </summary>
        /// <param name="SQL">
        ///     SQL command to be executed;
        ///     Comando SQL a ser executado;
        ///     (String)
        /// </param>
        /// <param name="sqlParameters">
        ///     Parameters to be used in the SQL command;
        ///     Parâmetros a serem usados no comando SQL;
        ///     (Nullable<List<MySql.Data.MySqlClient.MySqlParameter>>)
        /// </param>
        /// <returns>
        ///     Returns data according to the executed SQL command 
        ///     Retorna os dados de acordo com o comando SQL executado; 
        ///     (System.Data.DataSet)
        /// </returns>
        public DataSet ReadQuery(String SQL, List<MySqlParameter> mySQLParameters = null)
        {
            DataSet result = new DataSet();
            try
            {
                MySQLConnection.Open();
                using (MySqlCommand cmd = new MySqlCommand(SQL, MySQLConnection))
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
                SystemLog.WriteLog(String.Format("MySQL.ReadQuery; {0} {1} >> {2} {0} SQL:{3};", 
                    Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                MySQLConnection.Close();
            }
            return result;
        }

        /// <summary>
        ///     Method for executing SQL commands in which there is no data return;
        ///     Metodo para execução de comandos SQL nos quais não ha retorno de dados;
        /// </summary>
        /// <param name="SQL">
        ///     SQL command to be executed;
        ///     Comando SQL a ser executado;
        ///     (String)
        /// </param>
        /// <param name="sqlParameters">
        ///     Parameters to be used in the SQL command;
        ///     Parâmetros a serem usados no comando SQL;
        ///     (Nullable<List<MySql.Data.MySqlClient.MySqlParameter>>)
        /// </param>
        /// <returns>
        ///     If it fails, the method logs the error in the system log with the application name;
        ///     Caso falhe, o metodo registra o erro em log de sistema com o nome da aplicação;
        ///     (Boolean)
        /// </returns>
        public Boolean ExecuteQuery(String SQL, List<MySqlParameter> mySqlParameters = null)
        {
            Boolean result = false;
            try
            {
                MySQLConnection.Open();
                using (MySqlCommand cmd = new MySqlCommand(SQL, MySQLConnection))
                {
                    if (mySqlParameters != null)
                    {
                        cmd.Parameters.AddRange(mySqlParameters.ToArray());
                    }
                    cmd.ExecuteNonQuery();
                    result = true;
                }

                
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("MySQL.ExecuteQuery; {0} {1} >> {2} {0} SQL:{3};", Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                MySQLConnection.Close();
            }
            return result;
        }

    }
}
