
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
    
public partial class I_StockRequestItemCatalog
{

    public int Id { get; set; }

    public int ReqNo { get; set; }

    public int ItemCode { get; set; }

    public string ItemName { get; set; }

    public decimal Qty { get; set; }

    public int Status { get; set; }

    public string CreatedBy { get; set; }

    public System.DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedOn { get; set; }

    public int UnitId { get; set; }

    public string UnitName { get; set; }

}

}
