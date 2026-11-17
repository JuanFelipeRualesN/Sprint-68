using System;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure
{
    /// <summary>
    /// To handle the connections to the database, open them, close them, etc.
    /// </summary>
    class DBConnection
    {
        private string _connectionString;
        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// To open and get the connection.
        /// </summary>
        /// <returns>Returns the opened connection string.</returns>
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

        /// <summary>
        /// To close the DB connection.
        /// </summary>
        /// <param name="connection">The MySqlConnection to close.</param>
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
