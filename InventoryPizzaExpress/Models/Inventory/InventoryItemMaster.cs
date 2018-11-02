using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace InventoryPizzaExpress.Models

{
    public class InventoryItemMaster
    {
        public int Id { get; set; }
        public int ItemCode { get; set; }
        [Required]
        [Remote("doesItemNameExist", "InventoryItems", HttpMethod = "POST", ErrorMessage = "Item name already exists. Please enter a different Item name.")]
        public string ItemName { get; set; }
        public int MajorItemId { get; set; }
        public Nullable<int> MinorItemId { get; set; }
        public Nullable<int> PrimaryUnit { get; set; }
        public Nullable<decimal> PrimaryUnitConv { get; set; }
        public Nullable<int> ReportingUnit { get; set; }
        public Nullable<decimal> ReportingUnitConv { get; set; }
        public Nullable<int> StoreUnit { get; set; }
        public Nullable<decimal> StoreUnitConv { get; set; }
        public Nullable<decimal> LossPercentage { get; set; }
        public Nullable<decimal> GainPercentage { get; set; }
        public Nullable<decimal> ThersoldRecieving { get; set; }
        public Nullable<int> FixedorVariable { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

        public virtual Items I_ItemMater { get; set; }
        public virtual Items I_ItemMater1 { get; set; }
        public virtual Unit I_UnitMaster { get; set; }
        public virtual Unit I_UnitMaster1 { get; set; }
        public virtual Unit I_UnitMaster2 { get; set; }
    }
}