using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace MaestWebStore.Models
{
    public class User
    {
        /// <summary>
        /// A required username the user identifies themselves with.
        /// </summary>
        [Key]
        public int UserID { get; set; }

        /// <summary>
        /// A required username the user identifies themselves with.
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// The password (not hashed) the user used to log in.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //TODO: Perhaps add a new password confirmation field using:
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        /// <summary>
        /// A boolean which if checked leaves the user logged in.
        /// </summary>
        [Display(Name = "Remember me on this computer")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// The cart of games the user is about to buy, lasts per session.
        /// </summary>
        [Display(Name = "Cart")]
        public List<Game> Cart { get; set; }

        /// <summary>
        /// The display property of the cart
        /// </summary>
        [Display(Name = "Cart")]
        public string CartCount { get { if (Cart != null && Cart.Count != 0) return "Items:" + Convert.ToString(Cart.Count); else return "Cart is empty"; } }

        /// <summary>
        /// Checks wether the combination of a username and password exists in the database.
        /// </summary>
        /// <param name="_username">Username</param>
        /// <param name="_password">Password</param>
        /// <returns>A bool wether the user exists or not.</returns>
        public User IsValid(User user)
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                string _sqlSelect = "SELECT accountid, accountname FROM accounttable WHERE accountname = :accountname AND passwordhash = :passwordhash";
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                cmd.Parameters
                    .Add(new OracleParameter(":accountname", OracleDbType.NVarchar2))
                    .Value = user.Username;
                cmd.Parameters
                    .Add(new OracleParameter(":passwordhash", OracleDbType.NVarchar2))
                    .Value = Helpers.Hash.HashSHA1(user.Password);

                var dbReader = cmd.ExecuteReader();
                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        user.UserID = dbReader.GetInt32(0);
                        user.Cart = new List<Game>();
                    }

                    dbReader.Dispose();
                    cmd.Dispose();
                    return user;
                }
                else
                {
                    dbReader.Dispose();
                    cmd.Dispose();
                    return null;
                }
            }
            else
            {
                Debug.WriteLine("Database Error");
                return null;
            }

        }

        public bool IsRegistered(string _username, string _password)
        {
            string _sqlSelect = "SELECT accountname FROM accounttable WHERE accountname = :accountname";

            OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

            cmd.Parameters
                .Add(new OracleParameter(":accountname", OracleDbType.NVarchar2))
                .Value = _username;

            var dbReader = cmd.ExecuteReader();

            if (dbReader.HasRows)
            {
                dbReader.Dispose();
                cmd.Dispose();
                return false;
            }
            else
            {
                cmd.Dispose(); //Make the cmd ready for the next command

                string _sqlInsert = "INSERT INTO accounttable ( accountname, passwordhash ) VALUES ( :accountname, :passwordhash )";
                cmd = new OracleCommand(_sqlInsert, Util.DatabaseConnection.Conn);

                cmd.Parameters
                    .Add(new OracleParameter(":accountname", OracleDbType.NVarchar2))
                    .Value = _username;

                cmd.Parameters
                    .Add(new OracleParameter(":passwordhash", OracleDbType.NVarchar2))
                    .Value = Helpers.Hash.HashSHA1(_password);

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (OracleException oEx)
                {
                    Debug.WriteLine("Couldn't register: " + oEx.Message);
                    return false;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        /// <summary>
        /// Updates the user with the new given usermodel.
        /// </summary>
        /// <param name="user">The usermodel from the user that is logged in</param>
        /// <returns>A bool wether the operation was successfull</returns>
        public bool IsUpdated(User user)
        {
            string _sqlInsert = "UPDATE accounttable SET accountname = :accountname, passwordhash = :passwordhash WHERE accountid = :accountid";
            OracleCommand cmd = new OracleCommand(_sqlInsert, Util.DatabaseConnection.Conn);

            cmd.Parameters
                .Add(new OracleParameter(":accountname", OracleDbType.NVarchar2))
                .Value = user.Username;

            cmd.Parameters
                .Add(new OracleParameter(":passwordhash", OracleDbType.NVarchar2))
                .Value = Helpers.Hash.HashSHA1(user.Password);

            cmd.Parameters
                .Add(new OracleParameter(":passwordhash", OracleDbType.Int32))
                .Value = user.UserID;

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (OracleException oEx)
            {
                Debug.WriteLine("Couldn't update: " + oEx.Message);
                return false;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void AddWishlist(int gameId)
        {
            string _sqlInsert = "INSERT INTO account_wishlistgame ( accountid, appid ) VALUES ( :accountid, :appid )";
            OracleCommand cmd = new OracleCommand(_sqlInsert, Util.DatabaseConnection.Conn);

            cmd.Parameters
                .Add(new OracleParameter(":accountid", OracleDbType.NVarchar2))
                .Value = UserID;

            cmd.Parameters
                .Add(new OracleParameter(":appid", OracleDbType.NVarchar2))
                .Value = gameId;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException oEx)
            {
                Debug.WriteLine("Couldn't add games to wishlist: " + oEx.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void RemoveWishlist(int gameId)
        {
            string _sqlInsert = "DELETE FROM account_wishlistgame WHERE accountID = :accountid AND appid = :appid";
            OracleCommand cmd = new OracleCommand(_sqlInsert, Util.DatabaseConnection.Conn);

            cmd.Parameters
                .Add(new OracleParameter(":accountid", OracleDbType.NVarchar2))
                .Value = UserID;

            cmd.Parameters
                .Add(new OracleParameter(":appid", OracleDbType.NVarchar2))
                .Value = gameId;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException oEx)
            {
                Debug.WriteLine("Couldn'tremove games from wishlist: " + oEx.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

    }
}