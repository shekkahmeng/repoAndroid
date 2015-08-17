using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace ConferenceManagementSystem
{
    public class DBConnect
    {
        ///
        /// This class is used to connect to sql server database
        ///

        private static SqlConnection NewCon;
        private static string conStr = ConfigurationManager.ConnectionStrings["ConferenceManagementContext"].ConnectionString;

        public static SqlConnection getConnection()
        {
            NewCon = new SqlConnection(conStr);
            return NewCon;

        }
        public DBConnect()
        {

        }

    }
}