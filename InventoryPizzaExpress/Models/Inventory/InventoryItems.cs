using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Inventory
{
    public class InventoryItems
    {
        public int Id { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public int MajorItemId { get; set; }
        public string MajorItemName { get; set; }
        public Nullable<int> MinorItemId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string MinorItemName { get; set; }
        public string PrimaryUnitId { get; set; }
        public string PrimaryUnit { get; set; }
        public Nullable<decimal> PrimaryUnitConv { get; set; }
      
        public Nullable<int> FixedorVariable { get; set; }
        
    }
}