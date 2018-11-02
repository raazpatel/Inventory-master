using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models
{
    public class RecipeDetails
    {
        public int Id { get; set; }
        public Nullable<int> RecipeId { get; set; }
        public string MenuName { get; set; }
        public string RecipeName { get; set; }
        public string Article { get; set; }
        public string IngedientId { get; set; }
        public string Ingedient { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public string cost { get; set; }
     
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
}