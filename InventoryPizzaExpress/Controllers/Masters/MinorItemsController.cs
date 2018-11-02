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
using System.ComponentModel.DataAnnotations;

namespace InventoryPizzaExpress.Controllers
{
    [Authorize]
    public class MinorItemsController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: MinorItems
        public ActionResult Index()
        {
            ViewBag.MajorItems = from m in db.I_ItemMater
                                 where m.MajorItemId == null
                                 select new SelectListItem
                                 {

                                     Value = m.Id.ToString(),
                                     Text = m.ItemName

                                 };
        
            return View(db.I_ItemMater.Where(x => x.MajorItemId != null).Select(m=>new Items_Major
            {
                Id = m.Id,
                MajorItem = (from n in db.I_ItemMater where n.Id.ToString() == m.MajorItemId.ToString() select n.ItemName.ToString()).FirstOrDefault(),
                ItemDescription = m.ItemDescription,
                ItemName = m.ItemName,
                CreatedBy = m.CreatedBy,
                CreatedOn = m.CreatedOn,
                ModifiedBy = m.ModifiedBy,
                ModifiedOn = m.ModifiedOn,
                Status = m.Status
            } ).ToList());
        }

        // GET: MinorItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_ItemMater i_ItemMater = db.I_ItemMater.Find(id);
            if (i_ItemMater == null)
            {
                return HttpNotFound();
            }
            return View(i_ItemMater);
        }

        // GET: MinorItems/Create
        public ActionResult Create()
        {
            var _items = db.I_ItemMater.Where(x => x.MajorItemId == null);
            ViewBag.MajorItems = from m in _items
                                 where m.MajorItemId == null
                                 select new SelectListItem
                                 {

                                     Value = m.Id.ToString(),
                                     Text = m.ItemName
                                    
                                 };
            I_ItemMater i_ItemMater = new I_ItemMater();
            i_ItemMater.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_ItemMater.CreatedOn = System.DateTime.Now;
            return View(i_ItemMater);
        }
        [HttpPost]
        public ActionResult Index(int? MajorItemId, int? MajorItemId1)
        {

            ViewBag.MajorItems = from m in db.I_ItemMater
                                 where m.MajorItemId == null
                                 select new SelectListItem
                                 {

                                     Value = m.Id.ToString(),
                                     Text = m.ItemName

                                 };

           
            return View(db.I_ItemMater.Where(x => x.MajorItemId != null && x.MajorItemId== MajorItemId).Select(m => new Items_Major
            {
                Id = m.Id,
                MajorItem = (from n in db.I_ItemMater where n.Id.ToString() == m.MajorItemId.ToString() select n.ItemName.ToString()).FirstOrDefault(),
                ItemDescription = m.ItemDescription,
                ItemName = m.ItemName,
                CreatedBy = m.CreatedBy,
                CreatedOn = m.CreatedOn,
                ModifiedBy = m.ModifiedBy,
                ModifiedOn = m.ModifiedOn,
                Status = m.Status
            }).ToList());
          
        }
        // POST: MinorItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MajorItemId,ItemName,ItemDescription,Status,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_ItemMater i_ItemMater)
        {
            if (ModelState.IsValid)            {
             
                db.I_ItemMater.Add(i_ItemMater);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(i_ItemMater);
        }

        // GET: MinorItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

          


            I_ItemMater i_ItemMater = db.I_ItemMater.Find(id);
            int SelectedValue = Convert.ToInt32(i_ItemMater.MajorItemId);
            var _items = db.I_ItemMater.Where(x => x.MajorItemId == null);
            ViewBag.MajorItems = from m in _items
                                 where m.MajorItemId == null
                                 select new SelectListItem
                                 {

                                     Value = m.Id.ToString(),
                                     Text = m.ItemName,
                                     Selected = m.Id.ToString() == SelectedValue.ToString() ? true : false
                                 };
            if (i_ItemMater == null)
            {
                return HttpNotFound();
            }
            return View(i_ItemMater);
        }

        // POST: MinorItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MajorItemId,ItemName,ItemDescription,Status,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_ItemMater i_ItemMater)
        {
            if (ModelState.IsValid)
            {
                i_ItemMater.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_ItemMater.ModifiedOn = System.DateTime.Now;
                db.Entry(i_ItemMater).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(i_ItemMater);
        }

        // GET: MinorItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_ItemMater i_ItemMater = db.I_ItemMater.Find(id);
            if (i_ItemMater == null)
            {
                return HttpNotFound();
            }
            return View(i_ItemMater);
        }

        // POST: MinorItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                I_ItemMater i_ItemMater = db.I_ItemMater.Find(id);
                db.I_ItemMater.Remove(i_ItemMater);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
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
