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
using InventoryPizzaExpress.Models;
using InventoryPizzaExpress.Models.Inventory;

namespace InventoryPizzaExpress.Controllers.API.Minor
{
    public class MinorGroupController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/MinorGroup
        public IQueryable<Items_Major> GetMinorGroups()
        {
            return db.I_ItemMater.Where(x => x.MajorItemId != null).Select(m => new Items_Major
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
            });
        }
        // GET: api/MinorGroup
        public IQueryable<InventoryItems> GetItems(int id, int vendor)
        {
            IQueryable<InventoryItems> obj = db.I_InventoryItemMaster.Include(i => i.I_ItemMater1)
                  .Include(i => i.I_UnitMaster).Include(i => i.I_UnitMaster1)
                  .Include(i => i.I_UnitMaster2).Where(i => i.I_ItemMater1.Id == id).Select(m => new InventoryItems
                  {
                      Id = m.Id,
                      MinorItemId = m.I_ItemMater1.Id,
                      MinorItemName = m.I_ItemMater1.ItemName,
                      ItemCode = m.ItemCode,
                      ItemName = m.ItemName,
                      MajorItemId = m.I_UnitMaster.Id,
                      MajorItemName = m.I_ItemMater.ItemName,
                      PrimaryUnit = m.I_UnitMaster.UnitName,
                      PrimaryUnitId=m.PrimaryUnit.ToString(), 
                      FixedorVariable = m.FixedorVariable,
                      PrimaryUnitConv = m.PrimaryUnitConv,
                      Price = m.Price

                  });


            var val = from e in obj
            where !(from o in db.I_VendorItemCatalog where o.VendorId== vendor && o.Status==1
                    select o.ItemId)
                   .Contains(e.ItemCode)
            select e;
          
          


            return val;


        }



        // GET: api/MinorGroup/5
        [ResponseType(typeof(I_UnitMaster))]
        public IHttpActionResult GetI_UnitMaster(int id)
        {
            I_UnitMaster i_UnitMaster = db.I_UnitMaster.Find(id);
            if (i_UnitMaster == null)
            {
                return NotFound();
            }

            return Ok(i_UnitMaster);
        }

        // PUT: api/MinorGroup/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_UnitMaster(int id, I_UnitMaster i_UnitMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_UnitMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(i_UnitMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_UnitMasterExists(id))
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

        // POST: api/MinorGroup
        [ResponseType(typeof(I_UnitMaster))]
        public IHttpActionResult PostI_UnitMaster(I_UnitMaster i_UnitMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_UnitMaster.Add(i_UnitMaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_UnitMaster.Id }, i_UnitMaster);
        }

        // DELETE: api/MinorGroup/5
        [ResponseType(typeof(I_UnitMaster))]
        public IHttpActionResult DeleteI_UnitMaster(int id)
        {
            I_UnitMaster i_UnitMaster = db.I_UnitMaster.Find(id);
            if (i_UnitMaster == null)
            {
                return NotFound();
            }

            db.I_UnitMaster.Remove(i_UnitMaster);
            db.SaveChanges();

            return Ok(i_UnitMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool I_UnitMasterExists(int id)
        {
            return db.I_UnitMaster.Count(e => e.Id == id) > 0;
        }
    }
}