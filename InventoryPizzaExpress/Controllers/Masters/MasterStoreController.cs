using AutoMapper;
using InventoryPizzaExpress.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace InventoryPizzaExpress.Controllers.Masters
{
    [Authorize]
    public class MasterStoreController : Controller
    {

        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: Store
        public ActionResult Index()
        {
            return View(db.Store_Details.Where(m=>m.MasterStoreId==0).ToList());
        }
        [HttpPost]
        public JsonResult GetStoresDetails()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items = (from m in db.Store_Details  select new SelectListItem { Value = m.storeId.ToString(), Text = m.storename }).ToList();
            return Json(items);
        }
        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store_Details storeDetail = db.Store_Details.Find(id);
            if (storeDetail == null)
            {
                return HttpNotFound();
            }
            return View(storeDetail);
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoreDetails storeDetail)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<StoreDetails, Store_Details>();
                });
                storeDetail.MasterStoreId = 0;

                IMapper mapper = config.CreateMapper();

                var dest = mapper.Map<StoreDetails, Store_Details>(storeDetail);
                db.Store_Details.Add(dest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storeDetail);
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Store_Details storeDetail = db.Store_Details.Find(id);
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Store_Details, StoreDetails>();
            });

            IMapper mapper = config.CreateMapper();

            var dest = mapper.Map<Store_Details, StoreDetails>(storeDetail);
            if (storeDetail == null)
            {
                return HttpNotFound();
            }
            return View(dest);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StoreDetails storeDetail)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<StoreDetails, Store_Details>();
                });

                IMapper mapper = config.CreateMapper();

                var dest = mapper.Map<StoreDetails, Store_Details>(storeDetail);
                db.Entry(dest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storeDetail);
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store_Details storeDetail = db.Store_Details.Find(id);
            if (storeDetail == null)
            {
                return HttpNotFound();
            }
            return View(storeDetail);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Store_Details storeDetail = db.Store_Details.Find(id);
            db.Store_Details.Remove(storeDetail);
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