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
using InventoryPizzaExpress.Models.Waste;
using System.Threading.Tasks;

namespace InventoryPizzaExpress.Controllers
{[Authorize]
    public class WasteDetailsController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: WasteDetails
        public ActionResult Index()
        {
           
            var i_WasteDetails = (from m in db.I_WasteDetails.Include(i => i.Store_Details)
                                  join i in db.I_ItemMater.AsEnumerable() on m.WasteGroupId equals i.Id
                                 where m.Status !=-99
                                  select new WasteModel()
                                  {
                                        Id=m.Id,
                                        CreatedBy=m.CreatedBy,
                                        CreatedOn=m.CreatedOn,
                                         Date=m.Date,
                                         ModifiedBy=m.ModifiedBy,
                                         ModifiedOn = m.ModifiedOn,
                                         Reference=m.Reference,
                                         Status = m.Status,
                                         Store = m.Store_Details.storename,
                                         WasteGroup= i.ItemName,
                                         WasteNo =m.WasteNo
 
                                  });


            return View(i_WasteDetails.ToList());
        }
        [HttpGet]
        public ActionResult WasteReportByGroup(int? StoreId, int? ItemId, string Date)
        {
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename");
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_ItemMater.AsEnumerable().Where(x=>x.MajorItemId!=null).GroupBy(o => new { o.Id, o.ItemName }).Select(y => new SelectListItem
            {
                Text = y.First().ItemName,
                Value = y.First().Id.ToString(),
            }).Distinct().ToList();


            ViewBag.ItemId = new SelectList(item, "Value", "Text", ItemId);

            List<WasteReportByItem> list = new List<WasteReportByItem>();
            IEnumerable<I_WasteDetails> WasteList = db.I_WasteDetails.Include(i => i.Store_Details);
            IEnumerable<I_WasteItemCatalog> WasteListItems = db.I_WasteItemCatalog.AsEnumerable();
            if (StoreId != null)
            {
                WasteList = WasteList.Where(x => x.StoreId == StoreId);
            }
            if (Date != null && Date != "")
            {
                WasteList = WasteList.Where(x => (string.Format("{0:MM/dd/yyyy}", x.Date)) == (Date));
            }

            if (ItemId != null)
            {
                item = item.Where(x => x.Value == ItemId.ToString()).ToList();
            }

            var i_WasteDetails = (from m in WasteList.AsEnumerable()
                                  join i in item.AsEnumerable() on m.WasteGroupId.ToString() equals i.Value
                                 
                                  where m.Status != -99
                                  select new WasteReportByGroup()
                                  {
                                      Id = m.Id,                                   
                                      Date = (string.Format("{0:MM/dd/yyyy}", m.Date)),                                     
                                      Store = m.Store_Details.storename,
                                      WasteGroup = i.Text,
                                      WasteNo = m.WasteNo,
                                      value = (from wi in db.I_WasteItemCatalog where wi.WasteNo == m.Id select wi.Total).Sum().ToString()                                     

                                  });


            return View(i_WasteDetails.ToList());
        }
        [HttpGet]
        public ActionResult WasteReportByItem(int? StoreId, int? ItemId, string Date)
        {
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename");
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_WasteItemCatalog.AsEnumerable().GroupBy(o => new { o.ItemId, o.ItemName }).Select(y => new SelectListItem
            {
                Text = y.First().ItemName,
                Value = y.First().ItemId.ToString(),
            }).Distinct().ToList();


            ViewBag.ItemId = new SelectList(item, "Value", "Text", ItemId);

            List<WasteReportByItem> list = new List<WasteReportByItem>();
            IEnumerable<I_WasteDetails> WasteList = db.I_WasteDetails.Include(i => i.Store_Details);
            IEnumerable<I_WasteItemCatalog> WasteListItems = db.I_WasteItemCatalog.AsEnumerable();
            if (StoreId != null)
            {
                WasteList = WasteList.Where(x => x.StoreId == StoreId);
            }
            if (Date != null)
            {
                WasteList = WasteList.Where(x => (string.Format("{0:MM/dd/yyyy}", x.Date)) == (Date));
            }

            if (ItemId != null)
            {
                WasteListItems = WasteListItems.Where(x => x.ItemId == ItemId);
            }



            var i_WasteDetails = (from m in WasteList.AsEnumerable()
                                  join i in WasteListItems.AsEnumerable() on m.Id equals i.WasteNo
                                  where m.Status != -99
                                  group new { m, i } by new { m.Id,i.ItemId } into g
                                
                                  select new WasteReportByItem()
                                  {
                                      Id = g.FirstOrDefault().m.Id,
                                      Date = (string.Format("{0:MM/dd/yyyy}", g.FirstOrDefault().m.Date)),
                                      Store = g.FirstOrDefault().m.Store_Details.storename,
                                      ItemId = g.FirstOrDefault().i.ItemId,
                                      ItemName = g.FirstOrDefault().i.ItemName,
                                      WasteNo = g.FirstOrDefault().m.WasteNo,
                                      value = g.Sum(y=>y.i.Total).ToString()

                                  });


            return View(i_WasteDetails.ToList());
        }

        // GET: WasteDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_WasteDetails i_WasteDetails = db.I_WasteDetails.Find(id);
            if (i_WasteDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_WasteDetails.StoreId);
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_ItemMater.AsEnumerable().Where(x => x.MajorItemId != null).Select(y => new SelectListItem
            {
                Text = y.ItemName,
                Value = y.Id.ToString()
            }).ToList();


            ViewBag.WasteGroupId = new SelectList(item, "Value", "Text", i_WasteDetails.WasteGroupId);
            return View(i_WasteDetails);
        }

        // GET: WasteDetails/Create
        public ActionResult Create()
        {
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename");
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_ItemMater.AsEnumerable().Where(x => x.MajorItemId != null).Select(y => new SelectListItem
            {
                Text = y.ItemName,
                Value = y.Id.ToString()
            }).ToList();


            ViewBag.WasteGroupId = new SelectList(item, "Value", "Text");

            return View();
        }

        // POST: WasteDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( I_WasteDetails i_WasteDetails)
        {
            if (ModelState.IsValid)
            {
                i_WasteDetails.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_WasteDetails.CreatedOn = System.DateTime.Now;
                i_WasteDetails.Status = 1;
                i_WasteDetails.WasteNo = "WST";                
                db.I_WasteDetails.Add(i_WasteDetails);
                db.SaveChanges();
                I_WasteDetails obj = db.I_WasteDetails.Find(i_WasteDetails.Id);
                obj.WasteNo = "WST" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + i_WasteDetails.Id;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_WasteDetails.StoreId);
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_ItemMater.AsEnumerable().Where(x => x.MajorItemId != null).Select(y => new SelectListItem
            {
                Text = y.ItemName,
                Value = y.Id.ToString()
            }).ToList();


            ViewBag.WasteGroupId = new SelectList(item, "Value", "Text", i_WasteDetails.WasteGroupId);
            return View(i_WasteDetails);
        }

        // GET: WasteDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_WasteDetails i_WasteDetails = db.I_WasteDetails.Find(id);
            if (i_WasteDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_WasteDetails.StoreId);
            List<SelectListItem> item = new List<SelectListItem>();

            item = db.I_ItemMater.AsEnumerable().Where(x => x.MajorItemId != null).Select(y => new SelectListItem
            {
                Text = y.ItemName,
                Value = y.Id.ToString()
            }).ToList();


            ViewBag.WasteGroupId = new SelectList(item, "Value", "Text", i_WasteDetails.WasteGroupId);
            return View(i_WasteDetails);
        }

        // POST: WasteDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(I_WasteDetails i_WasteDetails)
        {
            if (ModelState.IsValid)
            {
                i_WasteDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_WasteDetails.ModifiedOn = System.DateTime.Now;
                i_WasteDetails.Status = 1;
                db.Entry(i_WasteDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StoreId = new SelectList(db.Store_Details, "storeId", "storename", i_WasteDetails.StoreId);
            List<SelectListItem> item = new List<SelectListItem>();
            item = db.I_ItemMater.AsEnumerable().Where(x => x.MajorItemId != null).Select(y => new SelectListItem
            {
                Text = y.ItemName,
                Value = y.Id.ToString()
            }).ToList();


            ViewBag.WasteGroupId = new SelectList(item, "Value", "Text", i_WasteDetails.WasteGroupId);
            return View(i_WasteDetails);
        }

        // GET: WasteDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_WasteDetails i_WasteDetails = db.I_WasteDetails.Find(id);
            if (i_WasteDetails == null)
            {
                return HttpNotFound();
            }
            i_WasteDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_WasteDetails.ModifiedOn = System.DateTime.Now;
            i_WasteDetails.Status = -99;
            db.Entry(i_WasteDetails).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
          
        }

        // POST: WasteDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_WasteDetails i_WasteDetails = db.I_WasteDetails.Find(id);
            db.I_WasteDetails.Remove(i_WasteDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddItems(List<I_WasteItemCatalog> WasteItems)
        {
            foreach (I_WasteItemCatalog item in WasteItems)
            {
                if (item.Status == 2)
                {
                    I_WasteDetails obj = new I_WasteDetails();
                    obj = db.I_WasteDetails.Find(item.WasteNo);
                    obj.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                    obj.ModifiedOn = System.DateTime.Now;
                    obj.Status = 2;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                }
            }

                foreach (I_WasteItemCatalog item in WasteItems)
            {


                I_WasteItemCatalog obj = new I_WasteItemCatalog();
                obj = db.I_WasteItemCatalog.Where(x=>x.WasteNo.ToString()== item.WasteNo.ToString() && x.ItemId==item.ItemId).SingleOrDefault();
                if (obj != null)
                {
                    db.I_WasteItemCatalog.Remove(obj);
                    db.SaveChanges();
                }              

                item.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                item.CreatedOn = System.DateTime.Now;               
                item.Cost = (db.I_StockInventory.Where(x => x.ItemId == item.ItemId).Select(Y => Y.Price).Average());
                item.Total = item.Qty * item.Cost;
                db.I_WasteItemCatalog.Add(item);
                db.SaveChanges();


            }

            return Json(new
            {
                success = true,
                data = ""
            },
             JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteConfirmed(List<int> id)
        {
            foreach (int item in id)
            {
                I_WasteItemCatalog i_WasteItemCatalog = db.I_WasteItemCatalog.Find(item);
                db.I_WasteItemCatalog.Remove(i_WasteItemCatalog);
                db.SaveChanges();
            }

            return Json("Success");
        }
        [HttpPost]
        public ActionResult GetMappedItems(int Req)
        {
            List<I_WasteItemCatalog> obj = new List<I_WasteItemCatalog>();
            obj = db.I_WasteItemCatalog.Where(x=>x.WasteNo ==Req).ToList();        

            return Json(obj,JsonRequestBehavior.AllowGet);
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
