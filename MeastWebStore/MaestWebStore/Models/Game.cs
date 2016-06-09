using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MaestWebStore.Models
{
    public class Game
    {
        [Display(Name = "App ID")]
        public int AppID { get; set; }

        [Display(Name = "App Type")]
        public string AppType { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Developer")]
        public string Developer { get; set; }

        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Display(Name = "Platform")]
        public string Platform { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Requirement ID")]
        public int ReqId { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Tags")]
        public string[] Tags { get; set; }


        public Game LoadGameID(int appID)
        {
            string _sqlSelect = "SELECT * FROM game WHERE appID = " + appID;
            OracleCommand cmd = new OracleCommand(_sqlSelect, Util.DatabaseConnection.Conn);

            var dbReader = cmd.ExecuteReader();

            AppID = appID;

            while (dbReader.Read())
            {
                AppType = dbReader.GetString(1);
                Name = dbReader.GetString(2);
                Developer = dbReader.GetString(3);
                Publisher = dbReader.IsDBNull(4) ? " " : dbReader.GetString(4); ;
                Platform = dbReader.GetString(5);
                Description = dbReader.IsDBNull(6) ? " " : dbReader.GetString(6);
                ReqId = dbReader.IsDBNull(7) ? 0 : dbReader.GetInt32(7);
                Price = dbReader.IsDBNull(8) ? -1 : dbReader.GetDouble(8);
                char delimiterChar = ',';
                Tags = dbReader.GetString(9).Split(delimiterChar);
            }
            cmd.Dispose();
            dbReader.Dispose();

            return this;
        }
    }
}