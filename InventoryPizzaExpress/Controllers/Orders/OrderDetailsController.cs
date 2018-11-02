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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using InventoryPizzaExpress.Models.Order;

namespace InventoryPizzaExpress.Controllers.Store
{
    public class OrderDetailsController : Controller
    {
      public  class AddIOrdertems
        {
            public int itemId { get; set; }
            public string ItemName { get; set; }
            public string ItemPrice { get; set; }
            public string Qty { get; set; }
            public int Unit { get; set; }
            public int status { get; set; }
            public int VendorId { get; set; }
            public int OrdNo { get; set; }
            public string total { get; set; }
            public int FixedorVariable { get; set; }
        }
        string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        private InventoryModuleEntities db = new InventoryModuleEntities();
        HttpClient client;
        string url = string.Empty;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public OrderDetailsController()
        {
            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: OrderDetails
        public ActionResult Index()
        {
            var i_OrderDetails = db.I_OrderDetails.Include(i => i.Store_Details);
            return View(i_OrderDetails.ToList());
        }
        public ActionResult OrderItemCatalog(int? id)
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
        // GET: OrderDetails/Details/5
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

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            I_OrderDetails i_OrderDetails = new I_OrderDetails();

            i_OrderDetails.CreatedBy = "Demo";
            i_OrderDetails.CreatedOn = System.DateTime.Now;
            i_OrderDetails.DeliveryDate = System.DateTime.Now;
            i_OrderDetails.OrderNo = "1";
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename");
            return View(i_OrderDetails);
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(I_OrderDetails i_OrderDetails)
        {
            if (ModelState.IsValid)
            {
                i_OrderDetails.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_OrderDetails.CreatedOn = System.DateTime.Now;                
                db.I_OrderDetails.Add(i_OrderDetails);
                db.SaveChanges();
                I_OrderDetails i_OrderDetail = db.I_OrderDetails.Find(i_OrderDetails.Id);
                i_OrderDetail.OrderNo = "ORD" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + i_OrderDetails.Id;

                i_OrderDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_OrderDetail.ModifiedOn = System.DateTime.Now;
                db.Entry(i_OrderDetail).State = EntityState.Modified;
                db.SaveChanges();
               
                return RedirectToAction("Index");
            }

            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_OrderDetails.StoreId);
            return View(i_OrderDetails);
        }

        // GET: OrderDetails/Edit/5
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
        [HttpPost]
        public async Task<JsonResult> GetItems(int value)
        {

            url = uri+"/api/OrderDetails/GetI_OrderDetails?id=" + value;
            List<Orders> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<Orders>>(responseData);
            }

            return Json(vendors);
        }
        [HttpPost]
        public async Task<JsonResult> GetOrderedItems()
        {

            url = uri + "/api/OrderDetails/GetFinalOrderedItems";
            List<FinalOrder> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<FinalOrder>>(responseData);
            }

            return Json(vendors);
        }
        [HttpPost]
        public async Task<JsonResult> GetOrderedItemsforReceiving(string orderIds)
        {

            url = uri + "/api/OrderDetails/GetOrderedItemsforReceiving?orderIds="+ orderIds;
            List<OrdersCustom> order = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                order = JsonConvert.DeserializeObject<List<OrdersCustom>>(responseData);
            }

            return Json(order);
        }
        [HttpPost]
        public async Task<JsonResult> GetSearchOrderedItems(string storeId,string vendorId, string Date)
        {



            url = uri + "/api/OrderDetails/GetSearchOrderedItems?storeId=" + storeId+ "&vendorId="+ vendorId+ "&Date="+ Date;
            List<FinalOrder> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<FinalOrder>>(responseData);
            }

            return Json(vendors);
        }
        [HttpPost]
        public async Task<ActionResult> AddItems(List<AddIOrdertems> listObject)
        {
            bool success = false;
           List<I_OrderItemCatalog> objNew = new List<I_OrderItemCatalog>();
            I_OrderItemCatalog obj;
            foreach (AddIOrdertems objItem in listObject)
            {
                obj = new I_OrderItemCatalog();


                obj.OrderId = objItem.OrdNo;
                obj.ItemsId = (objItem.itemId);
                obj.ItemName = (objItem.ItemName);
                obj.Price = Convert.ToDecimal(objItem.ItemPrice);
                obj.Status = (objItem.status);
                obj.Vendor = (objItem.VendorId);
                obj.Unit = (objItem.Unit);
                obj.TotalSum = Convert.ToDecimal(objItem.total);
                obj.Qty = Convert.ToDecimal(objItem.Qty);
                obj.FixedorVariable = objItem.FixedorVariable;
                obj.Createdby = System.Web.HttpContext.Current.User.Identity.Name;
                obj.CreatedOn = System.DateTime.Now;
                objNew.Add(obj);
            }
             url = uri+"/api/OrderDetails/PostOrderItems";
            client.BaseAddress = new Uri(url);
         
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, objNew);
            if (responseMessage.IsSuccessStatusCode)
            {
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


        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(I_OrderDetails i_OrderDetails)
        {
            if (ModelState.IsValid)
            {
                i_OrderDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_OrderDetails.ModifiedOn = System.DateTime.Now;
                db.Entry(i_OrderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_OrderDetails.StoreId);
            return View(i_OrderDetails);
        }

        // GET: OrderDetails/Delete/5
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

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_OrderDetails i_OrderDetails = db.I_OrderDetails.Find(id);
            db.I_OrderDetails.Remove(i_OrderDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
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
