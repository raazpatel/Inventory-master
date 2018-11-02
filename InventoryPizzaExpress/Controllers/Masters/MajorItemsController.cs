using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;

namespace InventoryPizzaExpress.Controllers
{
    [Authorize]
    public class MajorItemsController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: MajorItems
        public ActionResult Index()
        {
            return View(db.I_ItemMater.Where(x=>x.MajorItemId==null).ToList());
        }

        // GET: MajorItems/Details/5
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

        // GET: MajorItems/Create
        public ActionResult Create()
        {
            I_ItemMater i_ItemMater = new I_ItemMater();
            i_ItemMater.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_ItemMater.CreatedOn = System.DateTime.Now;
            return View(i_ItemMater);
        }

        // POST: MajorItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MajorItemId,ItemName,ItemDescription,Status,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_ItemMater i_ItemMater)
        {
          
            if (ModelState.IsValid)
            {
               
                db.I_ItemMater.Add(i_ItemMater);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(i_ItemMater);
        }

        // GET: MajorItems/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: MajorItems/Edit/5
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

        // GET: MajorItems/Delete/5
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

        // POST: MajorItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_ItemMater i_ItemMater = db.I_ItemMater.Find(id);
            db.I_ItemMater.Remove(i_ItemMater);
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
