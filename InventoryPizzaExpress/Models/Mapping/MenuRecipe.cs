using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Mapping
{
    public class MenuRecipe
    {
        public int v_mi_def_Id { get; set; }
        public int storeid { get; set; }
        public int mi_seq { get; set; }
        public Nullable<int> obj_num { get; set; }
        public string name_1 { get; set; }
        public string name_2 { get; set; }
        public Nullable<int> master_item_Id { get; set; }
        public string RecipeId { get; set; }
    }
}