using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Waste
{
    public class WasteReportByGroup
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        [DisplayName("Store Name")]
        public string Store { get; set; }
        public int WasteGroupId { get; set; }
        [DisplayName("Waste Group")]
        public string WasteGroup { get; set; }
        [DisplayName("Date")]    
        public string Date { get; set; }       
        public string WasteNo { get; set; }
        [DisplayName("Total")]
        public string value { get; set; }

    }
}