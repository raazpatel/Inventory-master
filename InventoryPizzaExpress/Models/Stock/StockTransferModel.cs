using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Stock
{
    public class StockTransferModel
    {
      
            public int Id { get; set; }
            public string SourceStore { get; set; }
            public string TragetStore { get; set; }
            public DateTime Date { get; set; }
        public string Status { get; set; }
        public string  TransferNo { get; set; }
        public string Reference { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<DateTime> ModifiedOn { get; set; }

        
    }
}