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
using System.Net.Http;
using System.Net.Http.Headers;

namespace InventoryPizzaExpress.Controllers
{[Authorize]
    public class StockTransferController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        HttpClient client;
        string url = string.Empty;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public StockTransferController()
        {
            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: StockTranfer
        public ActionResult Index()
        {
            var status = new Nullable<int>[] { 1,2 };
            IEnumerable<Store_Details> store = db.Store_Details.AsEnumerable();
           return View((from st in db.I_StockTranferDetails where status.Contains((st.Status))
                        select new StockTransferModel()
                         {
                             Id = st.Id,
                             SourceStore = (from s in store where s.storeId == st.SourceStore select s.storename).FirstOrDefault(),
                             TragetStore = (from s in store where s.storeId == st.TragetStore select s.storename).FirstOrDefault(),
                             Date =  st.Date,
                             Reference = st.Reference,
                             CreatedBy = st.CreatedBy,
                             CreatedOn = st.CreatedOn,
                             ModifiedBy = st.ModifiedBy,
                             ModifiedOn = st.ModifiedOn,
                             Status =st.Status.ToString(),
                             TransferNo = st.TranferNo

                         }).ToList());
        }
        public ActionResult PendingRequestDetails(int? id)
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
        public ActionResult PendingRequests()
        {
            IEnumerable<Store_Details> store = db.Store_Details.AsEnumerable();
            return View((from st in db.I_StockReqDetails
                         where st.Status == 2 // -99 - deleted, 1 - Saved ,2-hasItems, 3- approved, 4 - rejected ,
                         select new StockRequestModel()
                         {
                             Id = st.Id,
                             SourceStore = (from s in store where s.storeId == st.SourceStore select s.storename).FirstOrDefault(),
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

        // GET: StockTranfer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<Store_Details> store = db.Store_Details.AsEnumerable();
            return View((from st in db.I_StockTranferDetails
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

        // GET: StockTranfer/Create
        public ActionResult Create()
        {
            I_StockTranferDetails obj = new I_StockTranferDetails();
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            obj.Date = System.DateTime.Now;
            obj.TranferNo = "TRF";
            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename");
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename");
            return View(obj);
           
        }
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);

            i_StockReqDetails.Status = 3; // Approve
            i_StockReqDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockReqDetails.ModifiedOn = System.DateTime.Now;
            db.Entry(i_StockReqDetails).State = EntityState.Modified;
            db.SaveChanges();
            I_StockTranferDetails obj = new I_StockTranferDetails();
            obj.SourceStore = i_StockReqDetails.SourceStore;
            obj.TragetStore = i_StockReqDetails.TragetStore;
            obj.Reference = "Againest Request No " + i_StockReqDetails.TranferNo;
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            obj.Date = i_StockReqDetails.Date;
            obj.TranferNo = "TRN";
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            obj.Status = 1;
            db.I_StockTranferDetails.Add(obj);
            db.SaveChanges();
            I_StockTranferDetails i_StockTranferDetail = db.I_StockTranferDetails.Find(obj.Id);
            i_StockTranferDetail.TranferNo = "TRN" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + obj.Id;
            db.Entry(i_StockTranferDetail).State = EntityState.Modified;
            db.SaveChanges();
            List<I_StockRequestItemCatalog> i_StockRequestItemCatalog = new List<I_StockRequestItemCatalog>();

            i_StockRequestItemCatalog = db.I_StockRequestItemCatalog.Where(x => x.ReqNo == id).ToList();
            I_StockTransferItemCatalog objtransfer;
            foreach (I_StockRequestItemCatalog item in i_StockRequestItemCatalog)
            {
                objtransfer = new I_StockTransferItemCatalog();

                objtransfer.ItemCode = item.ItemCode;
                objtransfer.ItemName = item.ItemName;
                objtransfer.ReqNo = obj.Id;
                objtransfer.ReqQty = item.Qty;
                objtransfer.UnitId = item.UnitId;
                objtransfer.UnitName = item.UnitName;
                objtransfer.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                objtransfer.CreatedOn = System.DateTime.Now;
                db.I_StockTransferItemCatalog.Add(objtransfer);
                db.SaveChanges();
            }



            return RedirectToAction("PendingRequests");
        }
        class StockTransfer
        {
            public int Id { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public int UnitId { get; set; }
            public string UnitName { get; set; }
            public Nullable<decimal> Qty { get; set; }
            public decimal ReqQty { get; set; }

        }
        [HttpPost]
        public JsonResult GetMappedItems(int Req)
        {
            List<StockTransfer> obj = new List<StockTransfer>();
            obj = (from x in db.I_StockTransferItemCatalog
                   where (x.ReqNo == Req)
                   select new StockTransfer()
                   {
                       Id = x.Id,
                       ItemId = x.ItemCode,
                       ItemName = x.ItemName,
                       UnitId = x.UnitId,
                       UnitName = x.UnitName,
                       Qty =  (x.Qty),
                       ReqQty = x.ReqQty

                   }).ToList();
            return Json(obj);
        }
        [HttpPost]
        public JsonResult DeleteConfirmed(List<int> id)
        {
            foreach (int item in id)
            {
                I_StockTransferItemCatalog i_StockTransDetails = db.I_StockTransferItemCatalog.Find(item);
                db.I_StockTransferItemCatalog.Remove(i_StockTransDetails);
                db.SaveChanges();
            }

            return Json("Success");
        }
        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);

            i_StockReqDetails.Status = 4;// Reject
            i_StockReqDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockReqDetails.ModifiedOn = System.DateTime.Now;
            db.Entry(i_StockReqDetails).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("PendingRequests");
        }
        [HttpPost]
        public async Task<ActionResult> AddItems(List<I_StockTransferItemCatalog> StockRequest)
        {
            bool success = false;
            List<I_StockTransferItemCatalog> objNew = new List<I_StockTransferItemCatalog>();
            I_StockTransferItemCatalog obj;
            foreach (I_StockTransferItemCatalog objItem in StockRequest)
            {
                obj = new I_StockTransferItemCatalog();


                obj.ItemCode = objItem.ItemCode;
                obj.ItemName = (objItem.ItemName);
                obj.UnitId = (objItem.UnitId);
                obj.UnitName = objItem.UnitName;
                obj.Status = 1;
                obj.Qty = Convert.ToDecimal(objItem.Qty);
                obj.ReqQty = Convert.ToDecimal(objItem.ReqQty);
                obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                obj.CreatedOn = System.DateTime.Now;
                obj.ReqNo = objItem.ReqNo;

                objNew.Add(obj);
            }
            url = uri + "/api/APIStockReqDetails/PostI_StockTransferItemCatalog";
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

        // POST: StockTranfer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(I_StockTranferDetails i_StockTranferDetails)
        {
            i_StockTranferDetails.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockTranferDetails.CreatedOn = System.DateTime.Now;
            i_StockTranferDetails.Status = 1;
           
            if (ModelState.IsValid)
            {
                db.I_StockTranferDetails.Add(i_StockTranferDetails);
                db.SaveChanges();
                I_StockTranferDetails i_StockTranferDetail = db.I_StockTranferDetails.Find(i_StockTranferDetails.Id);
                i_StockTranferDetail.TranferNo = "TRN" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + i_StockTranferDetails.Id;
                Edit(i_StockTranferDetail);
                return RedirectToAction("Index");
            }
            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockTranferDetails.SourceStore);
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockTranferDetails.TragetStore);
            return View(i_StockTranferDetails);
        }

        // GET: StockTranfer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockTranferDetails i_StockTranferDetails = db.I_StockTranferDetails.Find(id);
            if (i_StockTranferDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockTranferDetails.SourceStore);
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockTranferDetails.TragetStore);
            return View(i_StockTranferDetails);
        }

        // POST: StockTranfer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( I_StockTranferDetails i_StockTranferDetails)
        {
            i_StockTranferDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockTranferDetails.ModifiedOn = System.DateTime.Now;
            if (ModelState.IsValid)
            {
                 db.Entry(i_StockTranferDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SourceStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockTranferDetails.SourceStore);
            ViewBag.TragetStore = new SelectList(db.Store_Details, "storeId", "storename", i_StockTranferDetails.TragetStore);
            return View(i_StockTranferDetails);
        }

        // GET: StockTranfer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockTranferDetails i_StockTranferDetails = db.I_StockTranferDetails.Find(id);

            i_StockTranferDetails.Status = -99;
            i_StockTranferDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_StockTranferDetails.ModifiedOn = System.DateTime.Now;
            db.Entry(i_StockTranferDetails).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
         
        }

        // POST: StockTranfer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_StockTranferDetails i_StockTranferDetails = db.I_StockTranferDetails.Find(id);
            db.I_StockTranferDetails.Remove(i_StockTranferDetails);
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
