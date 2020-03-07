using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Utilities.Log;

namespace Utilities.DataBaseConnection.SQLServer
{
    public class DataHandling
    {
        public Connection Connection { get; private set; }
        
        /// <summary>
        ///     Class for executing SQL commands using the SQLServer database;
        ///     Classe para a execução de comandos SQL utilizando o banco de dados SQLServer;
        /// </summary>
        /// <param name="connection">
        ///     Connection to the database;
        ///     Conexão com o banco de dados;
        ///     (Utilities.DataBaseConnection.SQLServer.Connection)
        /// </param>
        public DataHandling(Connection connection)
        {
            this.Connection = connection;
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
                Connection.SqlConnection.Open();
                result = true;
            }
            catch (Exception ex)
            {
                SystemLog.WriteLog(String.Format("SQLServer.ConnectionTest; {0} {1} >> {2}", Environment.NewLine, ex.Message, ex.StackTrace));
            }
            finally
            {
                Connection.SqlConnection.Close();
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
        ///     (Nullable<List<System.Data.SqlClient.SqlParameter>>)
        /// </param>
        /// <returns>
        ///     Returns data according to the executed SQL command 
        ///     Retorna os dados de acordo com o comando SQL executado; 
        ///     (System.Data.DataSet)
        /// </returns>
        public DataSet ReadQuery(String SQL, List<SqlParameter> sqlParameters = null)
        {
            DataSet result = new DataSet();
            try
            {
                Connection.SqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(SQL, Connection.SqlConnection))
                {

                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                        //foreach (SqlParameter parameter in sqlParameters)
                        //{
                        //    cmd.Parameters.Add(parameter);
                        //}
                    }
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    //dataAdapter.SelectCommand = cmd;
                    dataAdapter.Fill(result);
                }
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("SQLServer.ReadQuery; {0} {1} >> {2} {0} SQL:{3};", 
                    Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                Connection.SqlConnection.Close();
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
        ///     (Nullable<List<System.Data.SqlClient.SqlParameter>>)
        /// </param>
        /// <returns>
        ///     If it fails, the method logs the error in the system log with the application name;
        ///     Caso falhe, o metodo registra o erro em log de sistema com o nome da aplicação;
        ///     (Boolean)
        /// </returns>
        public Boolean ExecuteQuery(String SQL, List<SqlParameter> sqlParameters = null)
        {
            Boolean result = false;
            try
            {
                Connection.SqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(SQL, Connection.SqlConnection))
                {

                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters.ToArray());
                        //foreach (SqlParameter parameter in sqlParameters)
                        //{
                        //    cmd.Parameters.Add(parameter);
                        //}
                    }
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                SystemLog.WriteLog(String.Format("SQLServer.ExecuteQuery; {0} {1} >> {2} {0} SQL:{3};", Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                Connection.SqlConnection.Close();
            }
            return result;
        }
    }
}
