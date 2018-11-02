using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using InventoryPizzaExpress.Models;

using InventoryPizzaExpress.Models.Store;

namespace InventoryPizzaExpress.Controllers.Stock_Taking
{
    [Authorize]
    public class I_StockTakingController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();
        HttpClient client;
        string url = string.Empty;
        string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public I_StockTakingController()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: I_StockTaking
        public ActionResult Index()
        {
            //The URL of the WEB API Service
            return View();

        }
        [HttpPost]
        public JsonResult GetStoreDetails()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            url = uri + "/api/Stores/GetStore_Details";
            IEnumerable<Store_Details> vendors = null;
            vendors = db.Store_Details.ToList();           
            items = (from m in vendors select new SelectListItem { Value = m.storeId.ToString(), Text = m.storename }).ToList();
            return Json(items);
        }
        // GET: I_StockTaking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);

            Stock obj = new Stock();

            

            if (i_StockTaking == null)
            {
                return HttpNotFound();
            }

            obj.Id = i_StockTaking.Id.ToString();
            obj.Date = i_StockTaking.Date.ToString();
            obj.Count = CreateCountName(i_StockTaking.Id.ToString(), i_StockTaking.CreatedOn);
            obj.StoreName = i_StockTaking.Store_Details == null ? (from s in db.Store_Details where s.storeId == i_StockTaking.StoreId select s.storename).FirstOrDefault() : i_StockTaking.Store_Details.storename;

            return View(obj);
        }
        public ActionResult CountSheet(int? id, int? GroupId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (GroupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);

            Stock obj = new Stock();



            if (i_StockTaking == null)
            {
                return HttpNotFound();
            }

            obj.Id = i_StockTaking.Id.ToString();
            obj.Date = i_StockTaking.Date.ToString();
            obj.Count = CreateCountName(i_StockTaking.Id.ToString(), i_StockTaking.CreatedOn);
            obj.StoreName = i_StockTaking.Store_Details == null ? (from s in db.Store_Details where s.storeId == i_StockTaking.StoreId select s.storename).FirstOrDefault() : i_StockTaking.Store_Details.storename;
            obj.GroupId = GroupId.ToString();
            return View(obj);
        }

        // GET: I_StockTaking/Create
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetItems()
        {

            //url = uri + "/api/StockTaking/GetStockTaking";
            //List<I_StockTaking> _stockobj = null;

            //client.BaseAddress = new Uri(url);
            //HttpResponseMessage responseMessage = await client.GetAsync(url);
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            //    _stockobj = JsonConvert.DeserializeObject<List<I_StockTaking>>(responseData);
            //}
            List<Stock> obj = new List<Stock>();
             obj = (from m in db.I_StockTaking.Include(y => y.Store_Details).AsEnumerable()
                    select new Stock()
                     {
                         Id = m.Id.ToString(),
                         Count = CreateCountName(m.Id.ToString(), m.CreatedOn),
                         StoreName = m.Store_Details.storename,
                         Date = m.Date.ToShortDateString(),
                         InventoryTime = InventoryTime(m.InventoryTime.ToString()) ,
                         ClosingMethod = ClosingMethod(m.ClosingMethod.ToString()),
                         CreatedBy =m.CreatedBy,
                         CreatedOn=m.CreatedOn.ToShortDateString(),
                         ModifiedBy=m.ModifiedBy,
                         Modifiedon=m.ModifiedOn.ToString()


                     }).ToList();
           


            return Json(obj);
        }


        public class ItemDetails
        {
            public int Id { get; set; }
            public Nullable<int> ItemId { get; set; }
            public string ItemName { get; set; }
            public decimal ActualValue { get; set; }
            public decimal TheoValue { get; set; }
            public decimal Diff { get; set; }
            public decimal Counted { get; set; }
        }


        public class GroupItemDetails
        {
            public int Id { get; set; }
            public Nullable<int> ItemId { get; set; }
            public string ItemName { get; set; }
            public decimal VarQty { get; set; }
            public decimal Price { get; set; }
            public decimal Value { get; set; }
            public decimal ManualCount { get; set; }
        }

        [HttpPost]
        public ActionResult  GetItemGroupDetails(int id)
        {
            IEnumerable<Items_Major> objItem = (db.I_ItemMater.Where(x => x.MajorItemId != null).Select(m => new Items_Major
            {
                Id = m.Id,              
                ItemDescription = m.ItemDescription,
                ItemName = m.ItemName              
            })).AsEnumerable();
           List <ItemDetails> obj = new List<ItemDetails>();

            obj = (from m in db.I_StockTakingItemCatalog.AsEnumerable()
                   join n in db.I_StockTaking.AsEnumerable() on m.StockTakingId equals n.Id
                  
                   where m.StockTakingId == id
                   select new ItemDetails()
                   {
                       Id =m.Id,
                       ItemId = m.ItemGroupId,
                       ActualValue =Convert.ToDecimal(GetItemTotal(m.ItemGroupId,n.StoreId)),
                       ItemName = (from y in objItem where (y.Id == m.ItemGroupId) select (y.ItemName)).FirstOrDefault()
                   }).ToList();
            return Json(obj.Where(x=>x.ActualValue !=-99));
        }
        [HttpPost]
        public ActionResult GetGroupItems(int id)
        {
            IEnumerable<Items_Major> objItem = (db.I_InventoryItemMaster.Where(x => x.MinorItemId == id).Select(m => new Items_Major
            {
                Id = m.ItemCode,            
                ItemName = m.ItemName
            })).AsEnumerable();
            List<GroupItemDetails> obj = new List<GroupItemDetails>();

            obj = (from m in db.I_StockInventory.AsEnumerable()
                   join n in objItem on m.ItemId equals n.Id
                   group m by m.ItemId into g
                   select new GroupItemDetails()
                   {
                       Id = g.First().Id,
                       ItemId = g.First().ItemId,
                       ItemName = g.First().ItemName,
                       ManualCount = 0,
                       Price = g.Sum(_ => _.Price),
                       VarQty = 0,
                       Value = g.Sum(_ => _.Price) * 0
                   }).ToList();
           
            
            return Json(obj);
        }

        public  string GetItemTotal(int? GroupId, int StoreId)
        {

            string TotalValue = "-99";
            decimal? _valueInv =0 ;
            decimal? _valueTrans =0;
            IEnumerable<I_InventoryItemMaster> MappedItem ;
            MappedItem = (from n in db.I_InventoryItemMaster where (n.MinorItemId == (GroupId)) select n).AsEnumerable();
            if (MappedItem != null && MappedItem.GetEnumerator().MoveNext())
            {
                _valueInv = (from s in db.I_StockInventory join m in MappedItem on s.ItemId equals m.ItemCode where s.StoreId == StoreId select s.Qty*s.TotalAmount).Sum();

                _valueTrans = (from st in db.I_StockTranferDetails
                               join stItem in db.I_StockTransferItemCatalog on st.Id equals stItem.ReqNo
                               join m in MappedItem on stItem.ItemCode equals m.ItemCode
                               where st.SourceStore == StoreId 

                               select stItem.Qty * (db.I_StockInventory.Where(x => x.ItemId == stItem.ItemCode).Average(i => i.Price))*-1
                                ).Sum();
                TotalValue = Convert.ToString((_valueInv == null ? 0 : _valueInv) + (_valueTrans == null ? 0 : _valueTrans));
            }


           


            return TotalValue;

        }

        public string CreateCountName(string Id,DateTime createdOn)
        {
            string Val = "";
            if (createdOn != null)
            {
                Val = "INV" + createdOn.Year.ToString() + "/" + createdOn.Month + "/" + Id;
            }      

            return Val;
        }
        public string InventoryTime(string InventoryTime)
        {
            string Val = "";
            if (InventoryTime == "1")
            { Val = "Start of day(SOD)"; }
            if (InventoryTime == "2")
            { Val = "Mid-Day"; }
            if (InventoryTime == "3")
            { Val = "End of Day (EOD)"; }
            return Val;
        }
        public string ClosingMethod(string ClosingMethod)
        {
            string Val = "";
            if (ClosingMethod == "1")
            { Val = "Set not counted to 0"; }
            if (ClosingMethod == "2")
            { Val = "Set not counted to Theo. SOH"; }
           
            return Val;
        }
        [HttpPost]
        public async Task<ActionResult> AddItems(string StoreId, string Date, string InventoryTime, string ClosingMethod, string Items)
        {
            StockTaking stk = new StockTaking();

            I_StockTaking obj = new I_StockTaking();
            obj.StoreId = Convert.ToInt32(StoreId);
            obj.Date = Convert.ToDateTime(Date);
            obj.InventoryTime = Convert.ToInt32(InventoryTime);
            obj.ClosingMethod = Convert.ToInt32(ClosingMethod);
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            List<int> list = new List<int>();

            string[] arr = Items.Split(',');
            foreach (var item in arr)
            {
                if (item != "")
                    list.Add(Convert.ToInt32(item));
            }
            stk.st = obj;
            stk.Items = list;


            url = uri + "/api/StockTaking/PostI_StockTaking";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, stk);
            if (responseMessage.IsSuccessStatusCode)
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
        // POST: I_StockTaking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StoreId,Date,InventoryTime,InventoryTimeValue,ClosingMethod,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_StockTaking i_StockTaking)
        {
            if (ModelState.IsValid)
            {
                db.I_StockTaking.Add(i_StockTaking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_StockTaking.StoreId);
            return View(i_StockTaking);
        }

        // GET: I_StockTaking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);
            if (i_StockTaking == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_StockTaking.StoreId);
            return View(i_StockTaking);
        }

        // POST: I_StockTaking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StoreId,Date,InventoryTime,InventoryTimeValue,ClosingMethod,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_StockTaking i_StockTaking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(i_StockTaking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_StockTaking.StoreId);
            return View(i_StockTaking);
        }

        // GET: I_StockTaking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);
            if (i_StockTaking == null)
            {
                return HttpNotFound();
            }
            return View(i_StockTaking);
        }

        // POST: I_StockTaking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);
            db.I_StockTaking.Remove(i_StockTaking);
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








