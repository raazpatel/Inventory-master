using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models
{
    public class Items_Major
    {
        public int Id { get; set; }
        public string MajorItem { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}