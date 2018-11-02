using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models.Mapping;

namespace InventoryPizzaExpress.Controllers.Mapping
{
    [Authorize]
    public class MenuStoreMappingController : Controller
    {
        private  InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: MenuStoreMapping
        public ActionResult Index(int? obj_num)
        {
            List<MappedMenuItems> def = new List<MappedMenuItems>();
            List<UnMappedMenuItems> undef = new List<UnMappedMenuItems>();
            List<SelectListItem> items = new List<SelectListItem>();
            items = (from m in db.mi_def where m.storeid == 1001 select new SelectListItem { Value = m.obj_num.ToString(), Text = m.name_1 }).ToList();
            ViewBag.menuItems = items;
            if (obj_num == null || obj_num <= 0)
            {               
                def = (from m in db.mi_def where m.master_item_Id == 0 select  new MappedMenuItems {
                    name_1 = m.name_1,
                    obj_num = m.obj_num,
                    storeid = m.storeid
                }).ToList(); 
              }
            else            
            {
                def = (from m in db.mi_def
                       where m.master_item_Id == obj_num
                       select new MappedMenuItems
                       {
                           name_1 = m.name_1,
                           obj_num = m.obj_num,
                           storeid = m.storeid
                       }).ToList();

            }
            ViewBag.UnMappedStoredItems = (from m in db.mi_def
                                          where (m.master_item_Id == null || m.master_item_Id == 0) && m.storeid != 1001 
                                          select new UnMappedMenuItems
                                          {
                                              name_1 = m.name_1,
                                              obj_num = m.obj_num,
                                              storeid = m.storeid
                                          }).ToList();
            ViewBag.MappedStoredItems = def;
            return View(db.mi_def.Where(x=>x.storeid == 1001));
           
        }

        // GET: MenuStoreMapping/Details/5
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

        // GET: MenuStoreMapping/Create
        public ActionResult Create()
        {
          
            ViewBag.SourceStore = (from m in db.Store_Details where(m.storeId == 1001) select new SelectListItem { Value = m.storeId.ToString(), Text = m.storename }).ToList();
            ViewBag.TargetStore = (from m in db.Store_Details where (m.storeId != 1001) select new SelectListItem { Value = m.storeId.ToString(), Text = m.storename }).ToList();
            return View();
        }

        // POST: MenuStoreMapping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuStoreMapping storeDetail)
        {
            if (ModelState.IsValid)
            {
               int i = db.MapAllMenuItems( storeDetail.TargetStore);
               if(i>0)
                { return RedirectToAction("Index"); }
              
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
       public JsonResult ManualMapping(int SourcemenuId, int targetmenuId,int storeid)
        {
            mi_def mi_def = new mi_def();
            mi_def = (from m in db.mi_def where m.obj_num.ToString() == targetmenuId.ToString() && m.storeid== storeid select m).FirstOrDefault() ;
            mi_def.master_item_Id = SourcemenuId;
            db.Entry(mi_def).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Ok");
        }

        // GET: MenuStoreMapping/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: MenuStoreMapping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "storeId,storename")] Store_Details storeDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storeDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storeDetail);
        }

        // GET: MenuStoreMapping/Delete/5
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

        // POST: MenuStoreMapping/Delete/5
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
