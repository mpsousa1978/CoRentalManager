using MPSDataMananger.library.Models;
using MPSDataMananger.Library.Internal.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("[dbo].spProductGetAll", null, "MPSDataConnection");
            return output;
        }
    }
}
