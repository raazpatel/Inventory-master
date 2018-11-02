using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Waste
{
    public class WasteReportByItem
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        [DisplayName("Store Name")]
        public string Store { get; set; }
        [DisplayName("Item Code")]
        public int ItemId { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [DisplayName("Date")]
     
        public string Date { get; set; }
        public string WasteNo { get; set; }
        [DisplayName("Total")]
        public string value { get; set; }
    }
}