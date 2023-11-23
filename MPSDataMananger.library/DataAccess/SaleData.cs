
using MPSDataManager.library.Models;
using MPSDataMananger.library.Models;
using MPSDataMananger.Library.Internal.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.library.DataAccess
{
    public class SaleData
    {

        public void SaveSale(SaleModel saleInfo,string CashierId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            ProductData product = new ProductData();
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            var taxRate = ConfigHelper.GetTaxRate();
            foreach(var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };


                var producInfo = product.GetProductById(item.ProductId);
                if (producInfo == null )
                {
                    throw new Exception($"The product Id of {item.ProductId} coud not be found in the database");
                }
                detail.PurchasePrice = (producInfo.RetailPrice * detail.Quantity);
                if (producInfo.Istaxable)
                {
                    detail.Tax = (detail.PurchasePrice * (taxRate/100));
                }
                details.Add(detail);
            }


            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = CashierId
                
            };


            sale.Total = sale.SubTotal + sale.Tax;

            sql.SaveData("dbo.spSaleInsert",sale, "MPSDataConnection");

            //Get the ID from the sale mode
            sale.Id = sql.LoadData<int,dynamic>("dbo.spSaleLookUp", new {CashierId = sale.CashierId,SaleDate = sale.SaleDate}, "MPSDataConnection").FirstOrDefault();

            

            //finish filling i the sale detail model

            foreach ( var item in details )
            {
                item.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleDetailInsert", item, "MPSDataConnection");
            }


            
        }


    }
}
