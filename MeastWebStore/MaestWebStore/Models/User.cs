using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
            string _sqlSelect = @"SELECT accountname FROM accounttable WHERE accountname = ':accountname' AND passwordhash = ':passwordhash'";
            var cmd = new SqlCommand(_sqlSelect, Util.DatabaseConnection.Conn);
            cmd.Parameters
                .Add(new SqlParameter("accountname", SqlDbType.NVarChar))
                .Value = _username;
            cmd.Parameters
                .Add(new SqlParameter("password", SqlDbType.NVarChar))
                .Value = _password;
            Util.DatabaseConnection.OpenConnection();

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
    }
}