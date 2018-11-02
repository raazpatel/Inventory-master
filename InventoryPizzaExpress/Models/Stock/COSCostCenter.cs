using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Stock
{
    public class COSCostCenter
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public decimal OpeningValue { get; set; }
        public decimal Receipts { get; set; }
        public decimal Transfers { get; set; }
        public decimal Production { get; set; }
        public decimal Closing { get; set; }
        public decimal ActualCost { get; set; }
        public decimal ActualCostPec { get; set; }
        public decimal TheoCost { get; set; }
        public decimal TheoCostPec { get; set; }
        public decimal Variace { get; set; }
        public decimal VariacePec { get; set; }
        public decimal NetSales { get; set; }
        public int SalesDays { get; set; }
    }
}