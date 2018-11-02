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
    public class UnitsController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: Units
        public ActionResult Index()
        {
            return View(db.I_UnitMaster.ToList());
        }

        // GET: Units/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_UnitMaster i_UnitMaster = db.I_UnitMaster.Find(id);
            if (i_UnitMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_UnitMaster);
        }

        // GET: Units/Create
        public ActionResult Create()
        {
            I_UnitMaster i_UnitMaster = new I_UnitMaster();
           
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UnitName")] I_UnitMaster i_UnitMaster)
        {
            if (ModelState.IsValid)
            {
                db.I_UnitMaster.Add(i_UnitMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(i_UnitMaster);
        }

        // GET: Units/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_UnitMaster i_UnitMaster = db.I_UnitMaster.Find(id);
            if (i_UnitMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_UnitMaster);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UnitName")] I_UnitMaster i_UnitMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(i_UnitMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(i_UnitMaster);
        }

        // GET: Units/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_UnitMaster i_UnitMaster = db.I_UnitMaster.Find(id);
            if (i_UnitMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_UnitMaster);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_UnitMaster i_UnitMaster = db.I_UnitMaster.Find(id);
            db.I_UnitMaster.Remove(i_UnitMaster);
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
