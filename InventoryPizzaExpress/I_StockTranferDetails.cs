
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
    
public partial class I_StockTranferDetails
{

    public int Id { get; set; }

    public int SourceStore { get; set; }

    public int TragetStore { get; set; }

    public System.DateTime Date { get; set; }

    public string Reference { get; set; }

    public string CreatedBy { get; set; }

    public System.DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; }

    public Nullable<System.DateTime> ModifiedOn { get; set; }

    public Nullable<int> Status { get; set; }

    public string TranferNo { get; set; }



    public virtual I_StockTranferDetails I_StockTranferDetails1 { get; set; }

    public virtual I_StockTranferDetails I_StockTranferDetails2 { get; set; }

}

}
