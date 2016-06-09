﻿using System;
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

        /// <summary>
        /// A boolean which if checked leaves the user logged in.
        /// </summary>
        [Display(Name = "Remember me on this computer")]
        public bool RememberMe { get; set; }


        public bool IsValid(string _username, string _password)
        {
            if (Util.DatabaseConnection.IsDatabaseConnected)
            {
                //SELECT naam FROM account WHERE email = :accountname AND wachtwoord = :passwordhash
                //"SELECT accountname FROM accounttable WHERE accountname = :accountname AND passwordhash = :passwordhash"
                string _sqlSelect = "SELECT accountname FROM accounttable WHERE accountname = :accountname AND passwordhash = :passwordhash";
                OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

                cmd.Parameters
                    .Add(new OracleParameter(":accountname", OracleDbType.NVarchar2))
                    .Value = _username;
                cmd.Parameters
                    .Add(new OracleParameter(":passwordhash", OracleDbType.NVarchar2))
                    .Value = _password;

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
    }
}