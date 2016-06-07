using System;
using System.Data.SqlClient;

namespace MaestWebStore.Util
{
    public static class DatabaseConnection
    {
        /// <summary>
        /// The username used to connect with the database.
        /// </summary>
        private const string USER = "i319888";

        /// <summary>
        /// The password used to connect with the database.
        /// </summary>
        private const string PASSWORD = "Detunnel117!";

        /// <summary>
        /// The oracle-specific string used to connect with the database.
        /// </summary>
        //private const string CONNECTION_STRING = "User Id= " + USER + ";Password= " + PASSWORD + ";Data Source=" + @"//fhictora01.fhict.local" + ";";
        private const string CONNECTION_STRING = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fhictora01.fhict.local)(PORT=1521))(CONNECT_DATA =(SERVICE_NAME=fhictora)))" + "uid=" + USER + ";pwd=" + PASSWORD + ";";



        public static readonly SqlConnection Conn;

        static DatabaseConnection()
        {
            try
            {
                Conn = new SqlConnection(CONNECTION_STRING);
            }
            catch (SqlException oEx)
            {
                Console.WriteLine(@"Er kon geen verbinding met de Database worden gemaakt." + Environment.NewLine +
                                @"Controleer of er een internet en VPN verbinding is." + Environment.NewLine +
                                @"Error Message: " + oEx.Message + Environment.NewLine +
                                @"Error Data: " + oEx.Data);
            }
        }

        /// <summary>
        /// Opens a connection with the database.
        /// </summary>
        /// <returns>A bool wether the connection was sucessfully opened</returns>
        public static bool OpenConnection()
        {
            Console.WriteLine(Conn.State == System.Data.ConnectionState.Open ? "Database Connection was already open" : "Opening Database Connection..");
            try
            {
                Conn.Open();
                return true;
            }
            catch (SqlException oEx)
            {
                Console.WriteLine(@"Kon de databaseconnectie niet openen." + Environment.NewLine +
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
            catch (SqlException oEx)
            {
                Console.WriteLine(@"Kon de databaseconnectie niet sluiten." + Environment.NewLine +
                @"Controleer of er een internet en VPN verbinding is." + Environment.NewLine +
                @"Error Message: " + oEx.Message + Environment.NewLine +
                @"Error Data: " + oEx.Data);
                return false;
            }
        }
    }
}