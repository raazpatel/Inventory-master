using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Order
{
    public class FinalOrder
    {
        public int id { get; set; }
        public string OrderId{ get; set; }
        public string OrderName { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string Vendor { get; set; }
        public string VendorId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Total { get; set; }
        public string Reference { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }

       
    }
}