using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using System.Threading.Tasks;
using static InventoryPizzaExpress.Controllers.Store.OrderDetailsController;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using InventoryPizzaExpress.Models.Stock;

namespace InventoryPizzaExpress.Controllers.Receiving
{[Authorize]
    public class ReceivingController : Controller
    {
        string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        private InventoryModuleEntities db = new InventoryModuleEntities();
        HttpClient client;
        string url = string.Empty;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public ReceivingController()
        {
            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Receiving

     
        public ActionResult ItemReceived(string Datetime)
        {

            IEnumerable<ItemInStock> obj = (from x in db.I_StockInventory
                                            join v in db.I_VenderMaster on x.VendorId equals v.Id
                                            select new ItemInStock()
                                            {
                                                BusinessDate = x.BusinessDate,
                                                CreatedBy = x.CreatedBy,
                                                CreatedOn = x.CreatedOn,
                                                DiscountType = x.DiscountType,
                                                DiscountValue = x.DiscountValue,
                                                Id = x.Id,
                                                ItemId = x.ItemId,
                                                ItemName = x.ItemName,
                                                ModifiedBy = x.ModifiedBy,
                                                ModifiedOn = x.ModifiedOn,
                                                NetTotal = x.NetTotal,
                                                OrderNo = x.OrderNo,
                                                OrderType = x.OrderType,
                                                Price = x.Price,
                                                Qty = x.Qty,
                                                ReceiptNo = x.ReceiptNo,
                                                Ref = x.Ref,
                                                StoreId = x.StoreId,
                                                Tax = x.Tax,
                                                TotalAmount = x.TotalAmount,
                                                UnitId = x.UnitId,
                                                UnitName = x.UnitName,
                                                VendorId = x.VendorId,
                                                VendorName = v.VenderName
                                            } );

            return View(obj);

     
                                         
          

        }



        [HttpGet]
        public ActionResult Index(string Datetime)
        {
           
            if (Datetime!=null)
            {
                var j = db.I_OrderDetails.Include(i => i.Store_Details).Where(i => (string.Format("{0:dd/MM/yyyy}", i.DeliveryDate.ToShortDateString())) == (Datetime));
           return View(db.I_OrderDetails.Include(i => i.Store_Details).Where(i => (string.Format("{0:dd/MM/yyyy}", i.DeliveryDate.ToShortDateString())) == ( Datetime)));
            }
            else
            { return View(db.I_OrderDetails.Include(i => i.Store_Details).Where(i => i.Status == 1));
            }
       
        }

        [HttpGet]
        public ActionResult GetPendingOrders()
        {

            List<I_RecievingItemCatalog> obj = new List<I_RecievingItemCatalog>();
            List<I_RecievingItemCatalog> objnew = new List<I_RecievingItemCatalog>();
            obj = db.I_RecievingItemCatalog.ToList();

            int count = 0;

            foreach (I_RecievingItemCatalog item in obj)
            {
                count = 0;
                string[] arr = item.OrderIds.Split(',');

                foreach (var item1 in arr)
                {
                    var countStock = db.I_StockInventory.Where(x => x.OrderNo == item1).Count();
                    var countOrder = db.I_OrderItemCatalog.Where(x => x.I_OrderDetails.OrderNo == item1).Count();

                    if (countStock == countOrder)
                    {
                        count += 1;
                    }
                }

                if (arr.Length != count)
                {
                    objnew.Add(item);
                }
             

            }
            return View(objnew);          
        }
        // GET: Receiving/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_OrderDetails i_OrderDetails = db.I_OrderDetails.Find(id);
            if (i_OrderDetails == null)
            {
                return HttpNotFound();
            }
            return View(i_OrderDetails);
        }

        // GET: Receiving/Create
        public ActionResult Create()
        {
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename");
            return View();
        }
        [HttpGet]
        public ActionResult ReceivingItemCatalog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_RecievingItemCatalog i_RecievingItemCatalog = db.I_RecievingItemCatalog.Find(id);
            if (i_RecievingItemCatalog == null)
            {
                return HttpNotFound();
            }     
            return View(i_RecievingItemCatalog);
         
        }
        // POST: Receiving/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StoreId,DeliveryDate,Reference,OrderNo,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Status")] I_OrderDetails i_OrderDetails)
        {
            if (ModelState.IsValid)
            {
                db.I_OrderDetails.Add(i_OrderDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_OrderDetails.StoreId);
            return View(i_OrderDetails);
        }
        [HttpPost]
        public async Task<ActionResult> AddItems(List<AddOrdertems> listObject)
        {
            bool success = false;

            I_RecievingItemCatalog obj = new I_RecievingItemCatalog();
            foreach (AddOrdertems objItem in listObject)
            {
                obj.OrderIds = objItem.OrderIds;
                obj.DelieveryDate = (objItem.DelieveryDate);
                obj.Ref = (objItem.Ref);
                obj.ReceiptNo = objItem.ReceiptNo;
            }
            url = uri + "/api/RecievingItemCatalog/Post";
            client.BaseAddress = new Uri(url);

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, obj);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                obj = JsonConvert.DeserializeObject<I_RecievingItemCatalog>(responseData);
                success = true;
            }
            if (success)
            {
                return Json(new
                {
                    success = true,
                    data = obj
                },
            JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false,
                data = ""
            },
             JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public async Task<ActionResult> AddItems(List<AddIOrdertems> listObject)
        //{
        //    bool success = false;
        //    List<I_OrderItemCatalog> objNew = new List<I_OrderItemCatalog>();
        //    I_OrderItemCatalog obj;
        //    foreach (AddIOrdertems objItem in listObject)
        //    {
        //        obj = new I_OrderItemCatalog();


        //        obj.OrderId = objItem.OrdNo;
        //        obj.ItemsId = (objItem.itemId);
        //        obj.ItemName = (objItem.ItemName);
        //        obj.Price = Convert.ToDecimal(objItem.ItemPrice);
        //        obj.Status = (objItem.status);
        //        obj.Vendor = (objItem.VendorId);
        //        obj.Unit = (objItem.Unit);
        //        obj.TotalSum = Convert.ToDecimal(objItem.total);
        //        obj.Qty = Convert.ToDecimal(objItem.Qty);
        //        obj.FixedorVariable = objItem.FixedorVariable;
        //        obj.Createdby = System.Web.HttpContext.Current.User.Identity.Name;
        //        obj.CreatedOn = System.DateTime.Now;
        //        objNew.Add(obj);
        //    }
        //    url = uri + "/api/OrderDetails/PostOrderItems";
        //    client.BaseAddress = new Uri(url);

        //    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objNew);
        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        success = true;


        //    }
        //    if (success)
        //    {
        //        return Json(new
        //        {
        //            success = true
        //        },
        //    JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new
        //    {
        //        success = false
        //    },
        //     JsonRequestBehavior.AllowGet);
        //}
        // GET: Receiving/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_OrderDetails i_OrderDetails = db.I_OrderDetails.Find(id);
            if (i_OrderDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_OrderDetails.StoreId);
            return View(i_OrderDetails);
        }

        // POST: Receiving/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoreId,DeliveryDate,Reference,OrderNo,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Status")] I_OrderDetails i_OrderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(i_OrderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_OrderDetails.StoreId);
            return View(i_OrderDetails);
        }

        // GET: Receiving/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_OrderDetails i_OrderDetails = db.I_OrderDetails.Find(id);
            if (i_OrderDetails == null)
            {
                return HttpNotFound();
            }
            return View(i_OrderDetails);
        }

        // POST: Receiving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_OrderDetails i_OrderDetails = db.I_OrderDetails.Find(id);
            db.I_OrderDetails.Remove(i_OrderDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public partial class AddOrdertems
        {
            public int Id { get; set; }
            public string OrderIds { get; set; }
            public string ReceiptNo { get; set; }
            public string Ref { get; set; }
            public Nullable<System.DateTime> DelieveryDate { get; set; }
        }

        public partial class Ordertems
        {
            public string ReceiptNo { get; set; }
            public int VendorId { get; set; }
            public string OrderNo { get; set; }
            public string Ref { get; set; }
            public int itemId { get; set; }
            public string ItemName { get; set; }
            public int UnitId { get; set; }
            public string UnitName { get; set; }
            public decimal Price { get; set; }
            public decimal Qty { get; set; }
            public decimal NetTotal { get; set; }
            public int DiscountType { get; set; }
            public decimal DiscountValue { get; set; }
            public decimal Tax { get; set; }
            public decimal TotalAmount { get; set; }
            public System.DateTime BusinessDate { get; set; }
            public int StoreId { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> AddItems1(List<Ordertems> listObject)
        {
            bool success = false;

            I_StockInventory obj;
            List<I_StockInventory> list = new List<I_StockInventory>();
            foreach (Ordertems objItem in listObject)
            {
                obj=  new I_StockInventory();
                obj.OrderNo = objItem.OrderNo;
                obj.BusinessDate = (objItem.BusinessDate);
                obj.Ref = (objItem.Ref);
                obj.ReceiptNo = objItem.ReceiptNo;
                obj.DiscountType = objItem.DiscountType;
                obj.DiscountValue = objItem.DiscountValue;
                obj.ItemId = objItem.itemId;
                obj.ItemName = objItem.ItemName;
                obj.NetTotal = objItem.NetTotal;
                obj.OrderType = 1;// 1 mean Recieved Items
                obj.Price = objItem.Price;
                obj.Qty = objItem.Qty;
                obj.Tax = objItem.Tax;
                obj.TotalAmount = objItem.TotalAmount;
                obj.UnitId = objItem.UnitId;
                obj.UnitName = objItem.UnitName;
                obj.VendorId = objItem.VendorId;
                obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                obj.CreatedOn = System.DateTime.Now;
                obj.StoreId = objItem.StoreId;
                list.Add(obj);

            }
            url = uri + "/api/RecievingItemCatalog/PostItems";
            client.BaseAddress = new Uri(url);

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, list);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
            
                success = true;
            }
            if (success)
            {
                return Json(new
                {
                    success = true
                  
                },
            JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false
              
            },
            JsonRequestBehavior.AllowGet);
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
