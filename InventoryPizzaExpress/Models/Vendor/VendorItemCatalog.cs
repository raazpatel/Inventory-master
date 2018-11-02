using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Vendor
{
    public class VendorItemCatalog
    {
       
            public int Id { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public string ItemCode { get; set; }
            public decimal Price { get; set; }
            public int FixedorVariable { get; set; }
            public DateTime DateEffectiveFrom { get; set; }
        public DateTime DateEffectiveTo { get; set; }
        public string CreatedBy { get; set; }
            public System.DateTime CreatedOn { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public int Status { get; set; }
        public string PrimaryUnitId { get; set; }

        public string PrimaryUnit { get; set; }
    }
}