using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaestWebStore.Models
{
    public class StoreEntities
    {
        /// <summary>
        /// A list of the games in the game-catalog.
        /// </summary>
        public List<Game> Games { get; set; }

        public List<Game> Wishlist { get; set; }

        /// <summary>
        /// All the IDs currently in the game-catalog. Used to keep everything in line with the available games.
        /// </summary>
        private List<Tuple<int, int>> _GameIDs { get; set; }

        //Apps and users will go here too

        /// <summary>
        /// Fills the lists in this model with games.
        /// </summary>
        public void GetGames(/*TODO add search parameters*/)
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                _GameIDs = new List<Tuple<int, int>>();
                Games = new List<Game>();
                string _sqlSelect = "SELECT appid FROM game";
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    _GameIDs.Add(Tuple.Create(dbReader.GetInt32(0), 0));
                }
                cmd.Dispose();
                dbReader.Dispose();

                foreach (Tuple<int, int> i in _GameIDs)
                {
                    Games.Add(new Game().LoadGameID(i.Item1));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Database Error");
            }
        }

        //Gets the user's cart
        public void GetCart(User user)
        {
            Games = new List<Game>();

            foreach (Game game in user.Cart)
            {
                if (Util.DatabaseConnection.IsDatabaseConnected)
                {
                    Games.Add(new Game().LoadGameID(game.AppID));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Database Error");
                }
            }
        }
        //Gets the user's wishlist
        public void GetWishlist(User user)
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                _GameIDs = new List<Tuple<int, int>>();
                Wishlist = new List<Game>();
                string _sqlSelect = "SELECT appid FROM account_wishlistgame WHERE accountid = " + user.UserID;
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    _GameIDs.Add(Tuple.Create(dbReader.GetInt32(0), 0));
                }
                cmd.Dispose();
                dbReader.Dispose();

                foreach (Tuple<int, int> i in _GameIDs)
                {
                    Wishlist.Add(new Game().LoadGameID(i.Item1));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Database Error");
            }
        }
        //Gets the most bought games
        public void GetMostBought()
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                _GameIDs = new List<Tuple<int, int>>();
                Games = new List<Game>();
                string _sqlSelect = "SELECT appid, rownum FROM (SELECT appid, count(appid) AS bought FROM account_ownedgame GROUP BY appid ORDER BY bought desc) WHERE ROWNUM <= 4";
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    _GameIDs.Add(Tuple.Create(dbReader.GetInt32(0), dbReader.GetInt32(1)));
                }
                cmd.Dispose();
                dbReader.Dispose();

                foreach (Tuple<int, int> i in _GameIDs)
                {
                    Game game = new Game();
                    game.LoadGameID(i.Item1);
                    game.Rank = i.Item2;
                    Games.Add(game);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Database Error");
            }
        }
    }
}