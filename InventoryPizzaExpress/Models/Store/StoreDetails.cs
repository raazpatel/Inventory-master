using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Store
{
    public class StoreDetails
    {   [Required(ErrorMessage = "The Admin Master id is required")]
        //[Display(Name = "Admin Master ID")]
        public int storeId { get; set; }


        [Required(ErrorMessage = "The Admin Master is required")]
        //[Display(Name = "Admin Master")]
        public string storename { get; set; }

        [Display(Name = "Master Store Id")]
        public int MasterStoreId { get; set; }
      
        [Display(Name = "Address")]

        public string address { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Invalid mobile number")]
        public string phone { get; set; }
        [Display(Name = "Manager")]
        public string manager { get; set; }
    }
}