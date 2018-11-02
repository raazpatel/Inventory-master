using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models.Vendor;
using System.Globalization;

namespace InventoryPizzaExpress.Controllers.API.Vendor
{
    public class VendorController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/Vendor
        public IQueryable<I_VenderMaster> GetVendor()
        {
            return db.I_VenderMaster;
        }



        // GET: api/Vendor/5
        [ResponseType(typeof(I_VenderMaster))]
        public IHttpActionResult GetVendor(int id)
        {
            I_VenderMaster i_VenderMaster = db.I_VenderMaster.Find(id);
            if (i_VenderMaster == null)
            {
                return NotFound();
            }

            return Ok(i_VenderMaster);
        }
        [ResponseType(typeof(I_VendorItemCatalog))]
        public IHttpActionResult GetVendorItems(int id)
        {
            List<VendorItemCatalog> i_VenderMaster = new List<VendorItemCatalog>();
            try
            {
                i_VenderMaster = (from m in db.I_VendorItemCatalog
                                  where m.VendorId == id && m.Status == 1
                                  select new VendorItemCatalog()
                                  {
                                      Id = m.Id,
                                      ItemId = m.ItemId,
                                      ItemCode = m.ItemCode,
                                      DateEffectiveFrom = m.DateEffectiveFrom,
                                      DateEffectiveTo = m.DateEffectiveTo,
                                      ItemName = m.ItemName,
                                      CreatedBy = m.CreatedBy,
                                      CreatedOn = m.CreatedOn,
                                      FixedorVariable = m.FixedorVariable,
                                      ModifiedBy = m.ModifiedBy,
                                      ModifiedOn = m.ModifiedOn,
                                      Price = m.Price,
                                      PrimaryUnit = (from u in db.I_UnitMaster where u.Id == m.Unit select u.UnitName).FirstOrDefault(),
                                      PrimaryUnitId = m.Unit.ToString()
                                  }).ToList();
                if (i_VenderMaster == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return Ok(i_VenderMaster);
        }
        [ResponseType(typeof(I_VendorItemCatalog))]
        public IHttpActionResult GetVendorItemsOrder(int id, int order)
        {
            List<VendorItemCatalog> i_VenderMaster = new List<VendorItemCatalog>();
            try
            {
                List<int> orderList = (from o in db.I_OrderItemCatalog
                                       where o.Vendor == id && o.Status != -99 && o.OrderId == order
                                       select o.ItemsId).ToList();
                i_VenderMaster = (from m in db.I_VendorItemCatalog
                                  where m.VendorId == id && m.Status == 1 && !orderList.Contains(m.ItemId)
                                  select new VendorItemCatalog()
                                  {
                                      Id = m.Id,
                                      ItemId = m.ItemId,
                                      ItemCode = m.ItemCode,
                                      DateEffectiveFrom = m.DateEffectiveFrom,
                                      DateEffectiveTo = m.DateEffectiveTo,
                                      ItemName = m.ItemName,
                                      CreatedBy = m.CreatedBy,
                                      CreatedOn = m.CreatedOn,
                                      FixedorVariable = m.FixedorVariable,
                                      ModifiedBy = m.ModifiedBy,
                                      ModifiedOn = m.ModifiedOn,
                                      Price = m.Price,
                                      PrimaryUnit = (from u in db.I_UnitMaster where u.Id == m.Unit select u.UnitName).FirstOrDefault(),
                                      PrimaryUnitId = m.Unit.ToString()
                                  }).ToList();
                if (i_VenderMaster == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return Ok(i_VenderMaster);
        }



        // PUT: api/Vendor/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVender(int id, I_VenderMaster i_VenderMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_VenderMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(i_VenderMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_VenderMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutRemoveItem(int id, I_VendorItemCatalog obj1)
        {
            I_VendorItemCatalog obj = new I_VendorItemCatalog();

            obj = db.I_VendorItemCatalog.Find(id);
            obj.Status = 0;
            db.Entry(obj).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_VenderMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Vendor
        [ResponseType(typeof(I_VenderMaster))]
        public IHttpActionResult PostVender(I_VenderMaster i_VenderMaster)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_VenderMaster.Add(i_VenderMaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_VenderMaster.Id }, i_VenderMaster);
        }
        [ResponseType(typeof(I_VendorItemCatalog))]
        public IHttpActionResult PostVenderItems(I_VendorItemCatalog i_VenderMaster)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (I_VenderItemExists(i_VenderMaster.Id, i_VenderMaster.VendorId))
            {
                I_VendorItemCatalog obj = new I_VendorItemCatalog();
                obj = db.I_VendorItemCatalog.Find(i_VenderMaster.Id);
                obj.ItemCode = i_VenderMaster.ItemCode;
                obj.Price = i_VenderMaster.Price;
                obj.DateEffectiveFrom = i_VenderMaster.DateEffectiveFrom;
                obj.DateEffectiveTo = i_VenderMaster.DateEffectiveTo;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.I_VendorItemCatalog.Add(i_VenderMaster);
                db.SaveChanges();
            }
            return CreatedAtRoute("DefaultApi", new { id = i_VenderMaster.Id }, i_VenderMaster);
        }

        public IHttpActionResult PostVenderAddNewItems(I_VendorItemCatalog i_VenderMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.I_VendorItemCatalog.Add(i_VenderMaster);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = i_VenderMaster.Id }, i_VenderMaster);
        }

        // DELETE: api/Vendor/5
        [ResponseType(typeof(I_VenderMaster))]
        public IHttpActionResult DeleteI_VenderMaster(int id)
        {
            I_VenderMaster i_VenderMaster = db.I_VenderMaster.Find(id);
            if (i_VenderMaster == null)
            {
                return NotFound();
            }

            db.I_VenderMaster.Remove(i_VenderMaster);
            db.SaveChanges();

            return Ok(i_VenderMaster);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool I_VenderItemExists(int Id, int vendorid)
        {
            var val = db.I_VendorItemCatalog.Where(e => e.Id == Id && e.VendorId == vendorid && e.Status == 1).ToList();
            if (val.Count > 0)
                return true;
            else
                return false;
        }
        private bool I_VenderMasterExists(int id)
        {
            return db.I_VenderMaster.Count(e => e.Id == id) > 0;
        }
    }
}