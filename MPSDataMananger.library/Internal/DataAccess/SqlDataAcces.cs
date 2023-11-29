using Dapper;
using Microsoft.Extensions.Configuration;
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
    internal class SqlDataAccess :IDisposable //at the end close all connection
    {
        private readonly IConfiguration _config;
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString( string name)
        {
            return _config.GetConnectionString(name);
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


        private IDbConnection _connection;
        private IDbTransaction _transacion;




        public void StartTransaction(string ConnectionStringName)
        {
            string connectionStringName = GetConnectionString(ConnectionStringName);
            _connection = new SqlConnection(connectionStringName);
            _connection.Open();
            _transacion = _connection.BeginTransaction();

        }

        public List<T> LoadDataInTransaction<T, U>(string StoreProcedure, U Parameters)
        {
                List<T> rows = _connection.Query<T>(StoreProcedure, Parameters, 
                    commandType: CommandType.StoredProcedure,transaction: _transacion).ToList();

                return rows;
        }

        public void SaveDataInTransaction<T>(string StoreProcedure, T Parameters)
        {
               _connection.Execute(StoreProcedure, Parameters,
                    commandType: CommandType.StoredProcedure, transaction: _transacion);
        }

        private bool isCloserd = false;

        public IConfiguration Config { get; }

        public void CommitTransaction()
        {
            _transacion?.Commit();
            _connection?.Close();
            isCloserd = true;
        }

        public void RollBackTransaction()
        {
            _transacion?.Rollback();
            _connection?.Close();
            isCloserd = true;
        }

        public void Dispose()
        {
            if (isCloserd == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {

                    //TODO - Log this issue
                }
            }
            _transacion = null;
            _connection= null;
        }


        //open connect/start transaction method
        //load using the transaction
        //save using the transaction
        //Close connection/stop transaction metod
        //dispose
    }
}
