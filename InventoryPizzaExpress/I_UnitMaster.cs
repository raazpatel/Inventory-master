
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
    
public partial class I_UnitMaster
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public I_UnitMaster()
    {

        this.I_InventoryItemMaster = new HashSet<I_InventoryItemMaster>();

        this.I_InventoryItemMaster1 = new HashSet<I_InventoryItemMaster>();

        this.I_InventoryItemMaster2 = new HashSet<I_InventoryItemMaster>();

        this.I_OrderItemCatalog = new HashSet<I_OrderItemCatalog>();

        this.I_RecipeDetails = new HashSet<I_RecipeDetails>();

    }


    public int Id { get; set; }

    public string UnitName { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<I_InventoryItemMaster> I_InventoryItemMaster { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<I_InventoryItemMaster> I_InventoryItemMaster1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<I_InventoryItemMaster> I_InventoryItemMaster2 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<I_OrderItemCatalog> I_OrderItemCatalog { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<I_RecipeDetails> I_RecipeDetails { get; set; }

}

}