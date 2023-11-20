using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.Library.Internal.DataAcess

{
    internal class SqlDataAccess
    {
        public string GetConnectionString( string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string StoreProcedure, U Parameters, string ConnectionStringName)
        {
            string connectionStringName = GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionStringName))
            {
                List<T> rows = connection.Query<T>(StoreProcedure, Parameters, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string StoreProcedure, T Parameters, string ConnectionStringName)
        {
            string connectionStringName = GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionStringName))
            {
                connection.Execute(StoreProcedure, Parameters, 
                    commandType: CommandType.StoredProcedure);
            }
        }

    }
}
