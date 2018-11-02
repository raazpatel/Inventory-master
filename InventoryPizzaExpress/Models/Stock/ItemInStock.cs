using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Stock
{
    public class ItemInStock
    {
            public int Id { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public int UnitId { get; set; }
            public string UnitName { get; set; }
            public decimal Qty { get; set; }
            public decimal Price { get; set; }
            public decimal NetTotal { get; set; }
            public Nullable<int> DiscountType { get; set; }
            public Nullable<decimal> DiscountValue { get; set; }
            public Nullable<decimal> Tax { get; set; }
            public Nullable<decimal> TotalAmount { get; set; }
            public int OrderType { get; set; }
            public System.DateTime BusinessDate { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedOn { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public string ReceiptNo { get; set; }
            public Nullable<int> VendorId { get; set; }
        public string VendorName { get; set; }
        public string OrderNo { get; set; }
            public string Ref { get; set; }
            public Nullable<int> StoreId { get; set; }
             public String StoreName { get; set; }

    }
}