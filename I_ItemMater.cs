//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventoryPizzaExpress
{
    using System;
    using System.Collections.Generic;
    
    public partial class I_ItemMater
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_ItemMater()
        {
            this.I_InventoryItemMaster = new HashSet<I_InventoryItemMaster>();
            this.I_InventoryItemMaster1 = new HashSet<I_InventoryItemMaster>();
        }
    
        public int Id { get; set; }
        public Nullable<int> MajorItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_InventoryItemMaster> I_InventoryItemMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_InventoryItemMaster> I_InventoryItemMaster1 { get; set; }
    }
}
