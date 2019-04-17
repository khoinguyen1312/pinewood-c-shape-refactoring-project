using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PinnacleSample
{
    public interface ICustomerRepositoryDB
    {
        Customer GetByName(string name);
    }

    public class CustomerRepositoryDB : ICustomerRepositoryDB
    {
        public Customer GetByName(string name)
        {
            Customer _Customer = null;

            using (SqlConnection _Connection = RepositoryDBUtils.GetSqlConnection())
            {
                SqlCommand _Command = new SqlCommand
                {
                    Connection = _Connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "CRM_GetCustomerByName"
                };

                SqlParameter parameter = new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name };
                _Command.Parameters.Add(parameter);

                _Connection.Open();
                SqlDataReader _Reader = _Command.ExecuteReader(CommandBehavior.CloseConnection);
                while (_Reader.Read())
                {
                    _Customer = new Customer
                    {
                        ID = int.Parse(_Reader["CustomerID"].ToString()),
                        Name = _Reader["Name"].ToString(),
                        Address = _Reader["Address"].ToString()
                    };
                }
            }

            return _Customer;
        }
    }
}
