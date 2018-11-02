using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models
{
    public class StockTaking
    {
        public I_StockTaking st { get; set; }
        public List<int> Items { get; set; }
    }
}