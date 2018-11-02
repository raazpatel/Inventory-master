using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Store
{
    public class Stock
    {
        public string Id { get; set; }
        public string Count { get; set; }
        public string StoreName { get; set; }
        public string Date { get; set; }
        public string InventoryTime { get; set; }
        public string ClosingMethod { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string Modifiedon { get; set; }
        public string GroupId { get; set; }

    }
}