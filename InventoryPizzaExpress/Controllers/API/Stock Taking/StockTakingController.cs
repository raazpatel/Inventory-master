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

namespace InventoryPizzaExpress.Controllers.API.Stock_Taking
{
    public class StockTakingController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/StockTaking
        public IQueryable<I_StockTaking> GetStockTaking()
        {
            db.Database.CommandTimeout = 60;
            return db.I_StockTaking.Include(y => y.Store_Details);
        }

        // GET: api/StockTaking/5
        [ResponseType(typeof(I_StockTaking))]
        public IHttpActionResult GetI_StockTaking(int id)
        {
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);
            if (i_StockTaking == null)
            {
                return NotFound();
            }

            return Ok(i_StockTaking);
        }

        // PUT: api/StockTaking/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_StockTaking(int id, I_StockTaking i_StockTaking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_StockTaking.Id)
            {
                return BadRequest();
            }

            db.Entry(i_StockTaking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_StockTakingExists(id))
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

        // POST: api/StockTaking
        [ResponseType(typeof(I_StockTaking))]
        public IHttpActionResult PostI_StockTaking(StockTaking sk)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.I_StockTaking.Add(sk.st);
            db.SaveChanges();
            I_StockTakingItemCatalog StockTakingItemCatalog = new I_StockTakingItemCatalog();
            for (int i = 0; i < sk.Items.Count; i++)
            {
                StockTakingItemCatalog.StockTakingId = sk.st.Id;
                StockTakingItemCatalog.ItemGroupId = sk.Items[i];
                PostI_StockTakingItemCatalog(StockTakingItemCatalog);

            }
            return Ok(sk);
        }
        [ResponseType(typeof(I_StockTakingItemCatalog))]
        public IHttpActionResult PostI_StockTakingItemCatalog(I_StockTakingItemCatalog i_StockTakingItemCatalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_StockTakingItemCatalog.Add(i_StockTakingItemCatalog);
            db.SaveChanges();

           return Ok(i_StockTakingItemCatalog);
        }
        // DELETE: api/StockTaking/5
        [ResponseType(typeof(I_StockTaking))]
        public IHttpActionResult DeleteI_StockTaking(int id)
        {
            I_StockTaking i_StockTaking = db.I_StockTaking.Find(id);
            if (i_StockTaking == null)
            {
                return NotFound();
            }

            db.I_StockTaking.Remove(i_StockTaking);
            db.SaveChanges();

            return Ok(i_StockTaking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool I_StockTakingExists(int id)
        {
            return db.I_StockTaking.Count(e => e.Id == id) > 0;
        }
    }
}