using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.Models.Order
{
    public class Orders
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public int ItemsId { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> Unit { get; set; }
        public string UnitName { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> onHand { get; set; }
        public Nullable<decimal> onOrder { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string VendorName { get; set; }
        public Nullable<int> FixedorVariable { get; set; }
        public Nullable<decimal> TotalSum { get; set; }
        public Nullable<int> Status { get; set; }
        public string Createdby { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    }
    public class OrdersCustom
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public int ItemsId { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> Unit { get; set; }
        public string UnitName { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> onHand { get; set; }
        public Nullable<decimal> onOrder { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string VendorName { get; set; }
        public Nullable<int> FixedorVariable { get; set; }
        public Nullable<decimal> TotalSum { get; set; }
        public Nullable<int> Status { get; set; }
        public string Createdby { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Storeid { get; set; }
        public string StoreName { get; set; }
    }
}