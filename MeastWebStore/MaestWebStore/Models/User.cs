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
        /// Checks wether the combination of a username and password exists in the database.
        /// </summary>
        /// <param name="_username">Username</param>
        /// <param name="_password">Password</param>
        /// <returns>A bool wether the user exists or not.</returns>
        public bool IsValid(string _username, string _password)
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                
                string _sqlSelect = "SELECT accountname FROM accounttable WHERE accountname = :accountname AND passwordhash = :passwordhash";
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                cmd.Parameters
                    .Add(new OracleParameter(":accountname", OracleDbType.NVarchar2))
                    .Value = _username;
                cmd.Parameters
                    .Add(new OracleParameter(":passwordhash", OracleDbType.NVarchar2))
                    .Value = Helpers.Hash.HashSHA1(_password);

                var dbReader = cmd.ExecuteReader();
                if (dbReader.HasRows)
                {
                    dbReader.Dispose();
                    cmd.Dispose();
                    return true;
                }
                else
                {
                    dbReader.Dispose();
                    cmd.Dispose();
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("Database Error");
                return false;
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
    }
}