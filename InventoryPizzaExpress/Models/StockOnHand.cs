using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models
{
    public class StockOnHand
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

    }
}