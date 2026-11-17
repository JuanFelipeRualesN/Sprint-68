using System;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;

namespace SchoolsAPI.SchoolsAPI.Infrastructure
{
    /// <summary>
    /// Simple MySQL connection factory.
    /// </summary>
    /// <summary>
    /// To handle the connections to the database.
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
        /// To open and get the connection.
        /// </summary>
        /// <returns>Returns the MySqlConnection.</returns>
        public MySqlConnection GetConnection()
        {
            // Construct the connection with the connection string (fix)
            var connection = new MySqlConnection(_connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                // Optional: log the exception
            }

            return connection;
        }

        /// <summary>
        /// To close the connection.
        /// </summary>
        /// <param name="connection">The connection to close. </param>
        public void CloseConnection(MySqlConnection connection)
        {
            try
            {
                connection?.Close();
            }
            catch (Exception)
            {
                // Optional: log the exception
            }
        }
    }
}
