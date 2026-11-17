using System;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Infrastructure
{
    /// <summary>
    /// To handle the DB connection.
    /// </summary>
    public class DBConnection
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string of the DB.</param>
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// To get a connection and open it.
        /// </summary>
        /// <returns>Returns the connection to the DB.</returns>
        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = _connectionString
            };

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                // Ignore
            }

            return connection;
        }

        /// <summary>
        /// To close the connection to the DB.
        /// </summary>
        /// <param name="connection">The connection to close.</param>
        public void CloseConnection(MySqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception)
            {
                // Ignore.
            }
        }
    }
}
