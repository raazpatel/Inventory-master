using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Recipe
{
    public class RecipeItems
    {
        public int Id { get; set; }

        public Nullable<int> MemuItem { get; set; }

        public Nullable<int> RecipeId { get; set; }

        public Nullable<int> Article { get; set; }

        public Nullable<int> ItemId { get; set; }

        public Nullable<decimal> Qty { get; set; }

        public string CreatedBy { get; set; }

        public System.DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public virtual I_InventoryItemMaster I_InventoryItemMaster { get; set; }

        public virtual I_MenuItems I_MenuItems { get; set; }

        public virtual I_Recipe I_Recipe { get; set; }
    }
}