using System;
using MySqlConnector;

namespace TeachersAPI.CustomClasses
{
    /// <summary>
    /// Handles the DB Connection for the API.
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
        /// To get the connection. We know the g should b in Upper Case, but it is too late to change.
        /// </summary>
        /// <returns>Returns the connection.</returns>
        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = this._connectionString,
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
        /// To close the connection. We know the c should be in Upper Case, but is it too late.
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

            }
        }
    }
}
