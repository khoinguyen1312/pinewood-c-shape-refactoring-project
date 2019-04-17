using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PinnacleSample
{
    class RepositoryDBUtils
    {
        private static readonly string CONNECTION_STRING = "appDatabase";

        public static SqlConnection GetSqlConnection()
        {
            string _ConnectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString;

            return new SqlConnection(_ConnectionString);
        }
    }
}
