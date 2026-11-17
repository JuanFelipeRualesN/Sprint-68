using System;
using MySqlConnector;
using System.Configuration;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure
{
    /// <summary>
    /// To handle DB connections.
    /// </summary>
    class DBConnection
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// To open and get the connection string.
        /// </summary>
        /// <returns>The Mysql Connection.</returns>
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

            }

            return connection;
        }

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
