
using Microsoft.Extensions.Configuration;
using MPSDataManager.library.Models;
using MPSDataMananger.library.Models;
using MPSDataMananger.Library.Internal.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MPSDataMananger.library.DataAccess
{
    public class SaleData
    {
        private readonly IConfiguration _config;
        public SaleData(IConfiguration config)
        {
            _config = config;
        }
        public void SaveSale(SaleModel saleInfo,string CashierId)
        {
            
            ProductData product = new ProductData(_config);
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

            sale.Total = sale.SubTotal + sale.Tax;
            using (SqlDataAccess sql = new SqlDataAccess(_config))
            {
                try
                {
                    sql.StartTransaction("MPSDataConnection");
                    //save the sale model
                    sql.SaveDataInTransaction("dbo.spSaleInsert", sale);
                    //Get the ID from the sale mode
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookUp", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                    //finish filling i the sale detail model

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        //save the sale model
                        sql.SaveDataInTransaction("dbo.spSaleDetailInsert", item);
                    }

                    sql.CommitTransaction();
                }
                catch 
                {

                    sql.RollBackTransaction();
                    throw;
                }


            };

        }

        public List<SaleReportModel> GetSaleReposts() 
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.[spSale_SaleRepost]", new {}, "MPSDataConnection");
            return output;


        }

        //////****************sem transaction
        ////SqlDataAccess sql = new SqlDataAccess();

        //////save the sale model
        ////sql.SaveData("dbo.spSaleInsert",sale, "MPSDataConnection");

        //////Get the ID from the sale mode
        ////sale.Id = sql.LoadData<int,dynamic>("dbo.spSaleLookUp", new {CashierId = sale.CashierId,SaleDate = sale.SaleDate}, "MPSDataConnection").FirstOrDefault();

        //////finish filling i the sale detail model

        ////foreach ( var item in details )
        ////{
        ////    item.SaleId = sale.Id;
        ////    //save the sale model
        ////    sql.SaveData("dbo.spSaleDetailInsert", item, "MPSDataConnection");
        ////}
        ///
    }
}
