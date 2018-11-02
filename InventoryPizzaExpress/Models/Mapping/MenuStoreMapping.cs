using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Mapping
{
    public class MenuStoreMapping
    {[Key]
        [Required]
        public int SourceStore { get; set; }
        [Required]
        public int TargetStore { get; set; }
    }
}