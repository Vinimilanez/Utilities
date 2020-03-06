using System;
using System.Data.SqlClient;


namespace Utilities.DataBaseConnection.SQLServer
{
    public class Connection
    {
        public SqlConnection SqlConnection { get; private set; }

        public Connection(String dataSouce, String dataBase, String user, String password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = dataSouce;
            builder.UserID = user;
            builder.Password = password;
            builder.InitialCatalog = dataBase;

            this.SqlConnection = new SqlConnection(builder.ConnectionString.ToString());
        }
    }
}
