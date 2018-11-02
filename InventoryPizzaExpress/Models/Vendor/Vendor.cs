using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Vendor
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string VenderName { get; set; }
        [Required]
        public string AddressLine { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string GSTIN { get; set; }
       
        public string CreatedBy { get; set; }
       
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}