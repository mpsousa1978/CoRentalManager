using MPSDataMananger.library.Models;
using MPSDataMananger.Library.Internal.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MPSDataMananger.Library.DataAccess
{
    public  class UserData
    {
        private readonly IConfiguration _config;
        public UserData(IConfiguration config)
        {
            _config = config;
        }
        public List<UserModel> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);
            var p = new  { Id = Id };

            var output= sql.LoadData<UserModel,dynamic>("[dbo].spUserLookup", p, "MPSDataConnection");
            return output;
        }
    }
}

