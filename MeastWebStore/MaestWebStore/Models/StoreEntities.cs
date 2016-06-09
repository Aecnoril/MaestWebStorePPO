using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaestWebStore.Models
{
    public class StoreEntities
    {
        public List<Game> Games { get; set; }
        //Apps and users will go here too
        private List<int> _GameIDs { get; set; }

        public void GetGames(/*TODO add search parameters*/)
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                _GameIDs = new List<int>();
                Games = new List<Game>();
                string _sqlSelect = "SELECT appid FROM game";
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    _GameIDs.Add(dbReader.GetInt32(0));
                }
                cmd.Dispose();
                dbReader.Dispose();

                foreach(int i in _GameIDs)
                {
                    Games.Add(new Game().LoadGameID(i));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Database Error");
            }
        }

    }
}