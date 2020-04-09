using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Utilities.Log;

namespace Utilities.DataBaseConnection.Oracle
{
    public class DataHandling
    {
        public OracleConnection OracleConnection { get; private set; }

        public DataHandling(String dataSouce, String user, String password)
        {
            OracleConnectionStringBuilder builder = new OracleConnectionStringBuilder();

            builder.DataSource = dataSouce;
            builder.UserID = user;
            builder.Password = password;

            this.OracleConnection = new OracleConnection(builder.ConnectionString);
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
                OracleConnection.Open();
                result = true;
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("Oracle.ConnectionTest; {0} {1} >> {2}", Environment.NewLine, ex.Message, ex.StackTrace));
            }
            finally
            {
                OracleConnection.Close();
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
        ///     (Nullable<List<Oracle.ManagedDataAccess.Client.OracleParameter>>)
        /// </param>
        /// <returns>
        ///     Returns data according to the executed SQL command 
        ///     Retorna os dados de acordo com o comando SQL executado; 
        ///     (System.Data.DataSet)
        /// </returns>
        public DataSet ReadQuery(String SQL, List<OracleParameter> oracleParameters = null)
        {
            DataSet result = new DataSet();
            try
            {
                OracleConnection.Open();
                using (OracleCommand cmd = new OracleCommand(SQL, OracleConnection))
                {
                    if(oracleParameters != null)
                    {
                        cmd.Parameters.AddRange(oracleParameters.ToArray());
                    }
                    OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                    dataAdapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                SystemLog.WriteLog(String.Format("Oracle.ReadQuery; {0} {1} >> {2} {0} SQL:{3};", Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                OracleConnection.Close();
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
        ///     (Nullable<List<Oracle.ManagedDataAccess.Client.OracleParameter>>)
        /// </param>
        /// <returns>
        ///     If it fails, the method logs the error in the system log with the application name;
        ///     Caso falhe, o metodo registra o erro em log de sistema com o nome da aplicação;
        ///     (Boolean)
        /// </returns>
        public Boolean ExecuteQuery(String SQL, List<OracleParameter> oracleParameters = null)
        {
            Boolean result = false;
            try
            {
                OracleConnection.Open();
                using (OracleCommand cmd = new OracleCommand(SQL, OracleConnection))
                {
                    if(oracleParameters != null) 
                    {
                        cmd.Parameters.AddRange(oracleParameters.ToArray());
                    }
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch(Exception ex)
            {
                SystemLog.WriteLog(String.Format("Oracle.ExecuteQuery; {0} {1} >> {2} {0} SQL:{3};", Environment.NewLine, ex.Message, ex.StackTrace, SQL));
            }
            finally
            {
                OracleConnection.Close();
            }
            return result;
        }
    }
}
