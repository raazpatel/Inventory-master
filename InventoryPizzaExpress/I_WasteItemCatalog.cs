
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
    
public partial class I_WasteItemCatalog
{

    public int Id { get; set; }

    public int WasteNo { get; set; }

    public int ItemId { get; set; }

    public string ItemName { get; set; }

    public int UnitId { get; set; }

    public string UnitName { get; set; }

    public decimal Qty { get; set; }

    public string Reason { get; set; }

    public decimal Cost { get; set; }

    public decimal Total { get; set; }

    public int Status { get; set; }

    public string CreatedBy { get; set; }

    public System.DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedOn { get; set; }

}

}