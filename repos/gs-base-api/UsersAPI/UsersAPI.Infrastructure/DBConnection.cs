using System;
using MySqlConnector;

namespace UsersAPI.UsersAPI.Infrastructure
{
    /// <summary>
    /// To handle the DB connections.
    /// </summary>
    public class DBConnection
    {
        private string _connectionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// To open and get the database connection.
        /// </summary>
        /// <returns>Returns the database connection.</returns>
        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception)
            {

            }

            return connection;
        }

        /// <summary>
        /// To close the connection.
        /// </summary>
        /// <param name="connection">The connection in DB to close.</param>
        public void CloseConnection(MySqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}