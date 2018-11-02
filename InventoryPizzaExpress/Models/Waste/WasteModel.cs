using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Waste
{
    public class WasteModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Store { get; set; }
        public int WasteGroupId { get; set; }
        public string WasteGroup { get; set; }
        public System.DateTime Date { get; set; }
        public string Reference { get; set; }
        public string WasteNo { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> Status { get; set; }
    }
}