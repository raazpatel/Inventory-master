using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models;
using InventoryPizzaExpress.Models.Stock;

namespace InventoryPizzaExpress.Controllers
{[Authorize]
    public class StockOnHandController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: StockOnHand       
        [HttpGet]
        public ActionResult Index(int? StoreId, int? ItemId)
        {
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename");
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_StockInventory.AsEnumerable().GroupBy(o => new { o.ItemId, o.ItemName }).Select(y => new SelectListItem
            {
                Text = y.First().ItemName,
                Value = y.First().ItemId.ToString(),
            }).Distinct().ToList();


            ViewBag.ItemId = new SelectList(item, "Value", "Text", ItemId);
            List <StockOnHand> list = new List<StockOnHand>();
            IEnumerable<I_StockInventory> stockList = db.I_StockInventory.AsEnumerable();

            if (StoreId != null)
            {
                stockList = stockList.Where(x => x.StoreId == StoreId);
            }

            if (ItemId != null)
            {
                stockList = stockList.Where(x => x.ItemId == ItemId);
            }         
            
            list = (from i in stockList
                    group i by i.ItemId into g
                    select new StockOnHand()
                    {
                        ItemId = g.First().ItemId,
                        ItemName = g.First().ItemName,
                        Price = g.Sum(c => c.Price),
                        Qty = g.Sum(q => q.Qty),
                        Total = g.Sum(c => c.Price) * g.Sum(q => q.Qty),
                        UnitId = g.First().UnitId,
                        UnitName = g.First().UnitName
                    }).ToList();

            return View(list);
        }

        [HttpGet]
        public ActionResult COSByCostCenters()
        {
            List<COSCostCenter> list = new List<COSCostCenter>();
           COSCostCenter obj = new COSCostCenter();
            obj.StoreId = 1001;
            obj.StoreName = "Store 1";
            obj.OpeningValue = Convert.ToDecimal("4440004.00");
            obj.Closing = Convert.ToDecimal("4440004.00");
            list.Add(obj);
            COSCostCenter obj1 = new COSCostCenter();
            obj1.StoreId = 1001;
            obj1.StoreName = "Store 2";
            obj1.OpeningValue = Convert.ToDecimal("555555443.00");
            obj1.Closing = Convert.ToDecimal("555555443.00");
            list.Add(obj1);
            COSCostCenter obj2 = new COSCostCenter();
            obj2.StoreId = 1001;
            obj2.StoreName = "Store 3";
            obj2.OpeningValue = Convert.ToDecimal("887788111.00");
            obj2.Closing = Convert.ToDecimal("887788111.00");
            list.Add(obj2);
            COSCostCenter obj3 = new COSCostCenter();
            obj3.StoreId = 1001;
            obj3.StoreName = "Store 4";
            obj3.OpeningValue = Convert.ToDecimal("45455673.00");
            obj3.Closing = Convert.ToDecimal("45455673.00");
            list.Add(obj3);
            COSCostCenter obj4 = new COSCostCenter();
            obj4.StoreId = 1001;
            obj4.StoreName = "Store 5";
            obj4.OpeningValue = Convert.ToDecimal("67687954.00");
            obj4.Closing = Convert.ToDecimal("67687954.00");
            list.Add(obj4);
            COSCostCenter obj5 = new COSCostCenter();
            obj5.StoreId = 1001;
            obj5.StoreName = "Store 6";
            obj5.OpeningValue = Convert.ToDecimal("232324312.00");
            obj5.Closing = Convert.ToDecimal("232324312.00");
            list.Add(obj5);
            COSCostCenter obj6 = new COSCostCenter();
            obj6.StoreId = 1001;
            obj6.StoreName = "Store 7";
            obj6.OpeningValue = Convert.ToDecimal("97754322.00");
            obj6.Closing = Convert.ToDecimal("97754322.00");
            list.Add(obj6);
            COSCostCenter obj7 = new COSCostCenter();
            obj7.StoreId = 1001;
            obj7.StoreName = "Store 8";
            obj7.OpeningValue = Convert.ToDecimal("86323435.00");
            obj7.Closing = Convert.ToDecimal("86323435.00");
            list.Add(obj7);


            return View(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
