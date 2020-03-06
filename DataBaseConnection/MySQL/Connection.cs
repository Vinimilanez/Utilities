using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.DataBaseConnection.MySQL
{
    public class Connection
    {
        public MySqlConnection MySQLConnection { get; private set; }

        public Connection(String server, String dataBase, String user, String password)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

            builder.Database = dataBase;
            builder.Server = server;
            builder.UserID = user;
            builder.Password = password;

            this.MySQLConnection = new MySqlConnection(builder.ConnectionString);
        }

        public Connection(String server, String dataBase, String user, String password, uint port)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

            builder.Database = dataBase;
            builder.Server = server;
            builder.UserID = user;
            builder.Password = password;
            builder.Port = port;

            this.MySQLConnection = new MySqlConnection(builder.ConnectionString);
        }
    }
}
