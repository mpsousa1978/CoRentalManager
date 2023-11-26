using MPSDataMananger.library.Models;
using MPSDataMananger.Library.Internal.DataAcess;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.library.DataAccess
{
    public class InventoryData
    {

        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql=new SqlDataAccess();

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventoryGetAll", new { }, "MPSDataConnection");
            return output;
        }


        public void SaveInventoryRecord( InventoryModel inventory)
        {
            SqlDataAccess sql=new SqlDataAccess();

            sql.SaveData("dbo.spInventoryInsert", inventory, "MPSDataConnection");
        }
    }
}
