using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models.Store;
using AutoMapper;
using System.Threading.Tasks;

namespace InventoryPizzaExpress.Controllers.Store
{
    [Authorize]
    public class StoreController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: Store
        public ActionResult Index()
        {

            StoreDetails1 obj = new StoreDetails1();
            obj.StoreDetails1List = (from c1 in db.Store_Details
                        where c1.MasterStoreId == 0
                        join c2 in db.Store_Details on c1.storeId equals c2.MasterStoreId
                        select new StoreDetails1
                        {
                            storeId= c2.storeId,
                            storename= c2.storename,
                            address= c2.address,
                            city=  c2.city,
                            state= c2.state,
                            email= c2.email,
                            phone=c2.phone,
                            manager= c2.manager,
                            MasterStoreName = c1.storename
                        }).ToList();         
            //return View(db.Store_Details.Where(m => m.MasterStoreId != 0).ToList());        commented old qurey new query change by rajesh on 12-11-2018
            return View(obj);
        }
        [HttpPost]
        public  JsonResult GetStoresDetails()
        {
            List<SelectListItem> items = new List<SelectListItem>();
                    
            items = (from m in db.Store_Details select new SelectListItem { Value = m.storeId.ToString(), Text = m.storename }).ToList();
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
            //return View(db.Store_Details.Where(m => m.MasterStoreId == 0).ToList());
            ViewBag.MasterStoreList = from m in db.Store_Details
                                      where m.MasterStoreId == 0
                                      select new SelectListItem
                                      {
                                          Value = m.storeId.ToString(),
                                          Text = m.storename
                                      };


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
            ViewBag.MasterStoreList = from m in db.Store_Details
                                      where m.MasterStoreId == 0
                                      select new SelectListItem
                                      {
                                          Value = m.storeId.ToString(),
                                          Text = m.storename
                                      };


            Store_Details storeDetail = db.Store_Details.Find(id);
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Store_Details,StoreDetails >();
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
