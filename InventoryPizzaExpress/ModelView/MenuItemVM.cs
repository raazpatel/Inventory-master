using InventoryPizzaExpress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.ModelView
{
    public class MenuItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Recipeid { get; set; }

        public List<RecipeDetails> I_RecipeDetails { get; set; }
    }
}