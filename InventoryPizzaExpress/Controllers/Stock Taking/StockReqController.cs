using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models.Stock;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace InventoryPizzaExpress.Controllers.Stock_Taking
{[Authorize]
    public class StockReqController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();
        string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        HttpClient client;
        string url = string.Empty;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public StockReqController()
        {
            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: StockReq
        public ActionResult Index()
        {
            IEnumerable<Store_Details> store = db.Store_Details.AsEnumerable();
            return View((from st in db.I_StockReqDetails
                         where st.Status != -99 // -99 - deleted, 1 - Saved ,2-hasItems, 3- approved, 4 - rejected ,
                         select new StockRequestModel()
                         {
                             Id = st.Id,
                             SourceStoreId = st.SourceStore.ToString(),
                             SourceStore = (from s in store where s.storeId == st.SourceStore select s.storename).FirstOrDefault(),
                             TragetStoreId = st.TragetStore.ToString(),
                             TragetStore = (from s in store where s.storeId == st.TragetStore select s.storename).FirstOrDefault(),
                             Date = st.Date,
                             Reference = st.Reference,
                             CreatedBy = st.CreatedBy,
                             CreatedOn = st.CreatedOn,
                             ModifiedBy = st.ModifiedBy,
                             ModifiedOn = st.ModifiedOn,
                             Status = st.Status.ToString(),
                             TransferNo = st.TranferNo

                         }).ToList());

        }

        // GET: StockReq/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<Store_Details> store = db.Store_Details.AsEnumerable();
            return View((from st in db.I_StockReqDetails
                         where st.Id == id
                         select new StockRequestModel()
                         {
                             Id = st.Id,
                             SourceStoreId = st.SourceStore.ToString(),
                             SourceStore = (from s in store where s.storeId == st.SourceStore select s.storename).FirstOrDefault(),
                             TragetStoreId = st.TragetStore.ToString(),
                             TragetStore = (from s in store where s.storeId == st.TragetStore select s.storename).FirstOrDefault(),
                             Date = st.Date,
                             Reference = st.Reference,
                             CreatedBy = st.CreatedBy,
                             CreatedOn = st.CreatedOn,
                             ModifiedBy = st.ModifiedBy,
                             ModifiedOn = st.ModifiedOn,
                             Status = st.Status.ToString(),
                             TransferNo = st.TranferNo

                         }).SingleOrDefault());



        }

        // GET: StockReq/Create
        public ActionResult Create()
        {
            I_StockReqDetails obj = new I_StockReqDetails();
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            obj.Date = System.DateTime.Now;
            obj.TranferNo = "TRF";
            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename");
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename");
            return View(obj);

        }

        // POST: StockReq/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(I_StockReqDetails i_StockReqDetails)
        {
            i_StockReqDetails.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockReqDetails.CreatedOn = System.DateTime.Now;
            i_StockReqDetails.Status = 1;

            if (ModelState.IsValid)
            {
                db.I_StockReqDetails.Add(i_StockReqDetails);
                db.SaveChanges();
                I_StockReqDetails i_StockTranferDetail = db.I_StockReqDetails.Find(i_StockReqDetails.Id);
                i_StockTranferDetail.TranferNo = "REQ" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + i_StockReqDetails.Id;
                Edit(i_StockReqDetails);
                return RedirectToAction("Index");
            }
            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockReqDetails.SourceStore);
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockReqDetails.TragetStore);
            return View(i_StockReqDetails);

        }

        // GET: StockReq/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);
            if (i_StockReqDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockReqDetails.SourceStore);
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockReqDetails.TragetStore);
            return View(i_StockReqDetails);
        }

        // POST: StockReq/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(I_StockReqDetails i_StockReqDetails)
        {
            i_StockReqDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockReqDetails.ModifiedOn = System.DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(i_StockReqDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockReqDetails.SourceStore);
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockReqDetails.TragetStore);
            return View(i_StockReqDetails);



        }
        class StockRequest
        {
            public int Id { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public int UnitId { get; set; }
            public string UnitName { get; set; }
            public decimal Qty { get; set; }

        }

        [HttpPost]
        public JsonResult GetItems(int Store)
        {
            List<StockRequest> obj = new List<StockRequest>();
            obj = (from x in db.I_StockInventory
                   where (x.StoreId == Store)
                   select new StockRequest()
                   {
                       ItemId = x.ItemId,
                       ItemName = x.ItemName,
                       UnitId = x.UnitId,
                       UnitName = x.UnitName
                   }).Distinct().ToList();
            return Json(obj);
        }

        class values
        {
            public int id { get; set; }
        }

        [HttpPost]
        public JsonResult GetMappedItems(int Req)
        {
            List<StockRequest> obj = new List<StockRequest>();
            obj = (from x in db.I_StockRequestItemCatalog
                   where (x.ReqNo == Req)
                   select new StockRequest()
                   {   Id = x.Id, 
                       ItemId = x.ItemCode,
                       ItemName = x.ItemName,
                       UnitId = x.UnitId,
                       UnitName = x.UnitName,
                       Qty = x.Qty
                   }).Distinct().ToList();
            return Json(obj);
        }
        [HttpPost]
        public JsonResult DeleteConfirmed(List<int> id)
        {
            foreach (int item in id)
            {
                I_StockRequestItemCatalog i_StockReqDetails = db.I_StockRequestItemCatalog.Find(item);
                db.I_StockRequestItemCatalog.Remove(i_StockReqDetails);
                db.SaveChanges();
            }
           
            return Json("Success");
        }
        [HttpPost]
        public async Task<ActionResult> AddItems(List<I_StockRequestItemCatalog> StockRequest)
        {
            bool success = false;
            List<I_StockRequestItemCatalog> objNew = new List<I_StockRequestItemCatalog>();
            I_StockRequestItemCatalog obj;
            foreach (I_StockRequestItemCatalog objItem in StockRequest)
            {
                obj = new I_StockRequestItemCatalog();


                obj.ItemCode = objItem.ItemCode;
                obj.ItemName = (objItem.ItemName);
                obj.UnitId = (objItem.UnitId);
                obj.UnitName = objItem.UnitName;
                obj.Status =1;               
                obj.Qty = Convert.ToDecimal(objItem.Qty);
                obj.CreatedBy= System.Web.HttpContext.Current.User.Identity.Name;
                obj.CreatedOn = System.DateTime.Now;
                obj.ReqNo = objItem.ReqNo;
                
                objNew.Add(obj);
            }
            url = uri + "/api/APIStockReqDetails/PostI_StockRequestItemCatalog";
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
        // GET: StockReq/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);

            i_StockReqDetails.Status = -99;
            i_StockReqDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockReqDetails.ModifiedOn = System.DateTime.Now;
            db.Entry(i_StockReqDetails).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: StockReq/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed1(int id)
        {
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);
            db.I_StockReqDetails.Remove(i_StockReqDetails);
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
