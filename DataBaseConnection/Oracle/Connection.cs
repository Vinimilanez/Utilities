using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.DataBaseConnection.Oracle
{
    public class Connection
    {
        public OracleConnection OracleConnection { get; private set; }

        public Connection(String dataSouce, String user, String password)
        {
            OracleConnectionStringBuilder builder = new OracleConnectionStringBuilder();

            builder.DataSource = dataSouce;
            builder.UserID = user;
            builder.Password = password;

            this.OracleConnection = new OracleConnection(builder.ConnectionString);
         }

    }
}
