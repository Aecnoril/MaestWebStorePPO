using System;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace MaestWebStore.Util
{
    public static class DatabaseConnection
    {
        /// <summary>
        /// The oracle-specific string used to connect with the database.
        /// </summary>
        //private const string CONNECTION_STRING = "User Id= " + USER + ";Password= " + PASSWORD + ";Data Source=" + @"//fhictora01.fhict.local" + ";";
        private const string _CONNECTION_STRING = "Data Source={0};User Id={1};Password={2};";

        public static OracleConnection Conn;

        public static bool IsDatabaseConnected => Conn != null && Conn.State == System.Data.ConnectionState.Open;

        public static bool Initialize(string user, string password, string database)
        {
            Conn = new OracleConnection(string.Format(_CONNECTION_STRING, database, user, password));
            return OpenConnection();
        }

        /// <summary>
        /// Opens a connection with the database.
        /// </summary>
        /// <returns>A bool wether the connection was sucessfully opened</returns>
        public static bool OpenConnection()
        {
            Debug.WriteLine(Conn.State == System.Data.ConnectionState.Open ? "Database Connection was already open" : "Opening Database Connection..");
            try
            {
                Conn.Open();
                return true;
            }
            catch (OracleException oEx)
            {
                Debug.WriteLine(@"Kon de databaseconnectie niet openen." + Environment.NewLine +
                @"Controleer of er een internet en VPN verbinding is." + Environment.NewLine +
                @"Error Message: " + oEx.Message + Environment.NewLine +
                @"Error Data: " + oEx.Data);
                return false;
            }
        }

        /// <summary>
        /// Closes the connection with the database.
        /// </summary>
        /// <returns>A bool wether the connection was successfully closed</returns>
        public static bool CloseConnection()
        {
            Console.WriteLine(Conn.State == System.Data.ConnectionState.Closed ? "Database Connection was already closed" : "Closing Database Connection..");
            try
            {
                Conn.Close();
                return true;
            }
            catch (OracleException oEx)
            {
                Debug.WriteLine(@"Kon de databaseconnectie niet sluiten." + Environment.NewLine +
                @"Controleer of er een internet en VPN verbinding is." + Environment.NewLine +
                @"Error Message: " + oEx.Message + Environment.NewLine +
                @"Error Data: " + oEx.Data);
                return false;
            }
        }
    }
}