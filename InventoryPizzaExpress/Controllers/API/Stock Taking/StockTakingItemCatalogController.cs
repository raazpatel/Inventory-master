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

namespace InventoryPizzaExpress.Controllers.API.Stock_Taking
{
    public class StockTakingItemCatalogController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/StockTakingItemCatalog
        public IQueryable<I_StockTakingItemCatalog> GetI_StockTakingItemCatalog()
        {
            return db.I_StockTakingItemCatalog;
        }

        // GET: api/StockTakingItemCatalog/5
        [ResponseType(typeof(I_StockTakingItemCatalog))]
        public IHttpActionResult GetI_StockTakingItemCatalog(int id)
        {
            I_StockTakingItemCatalog i_StockTakingItemCatalog = db.I_StockTakingItemCatalog.Find(id);
            if (i_StockTakingItemCatalog == null)
            {
                return NotFound();
            }

            return Ok(i_StockTakingItemCatalog);
        }

        // PUT: api/StockTakingItemCatalog/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_StockTakingItemCatalog(int id, I_StockTakingItemCatalog i_StockTakingItemCatalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_StockTakingItemCatalog.Id)
            {
                return BadRequest();
            }

            db.Entry(i_StockTakingItemCatalog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_StockTakingItemCatalogExists(id))
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

        // POST: api/StockTakingItemCatalog
        [ResponseType(typeof(I_StockTakingItemCatalog))]
        public IHttpActionResult PostI_StockTakingItemCatalog(I_StockTakingItemCatalog i_StockTakingItemCatalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_StockTakingItemCatalog.Add(i_StockTakingItemCatalog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_StockTakingItemCatalog.Id }, i_StockTakingItemCatalog);
        }

        // DELETE: api/StockTakingItemCatalog/5
        [ResponseType(typeof(I_StockTakingItemCatalog))]
        public IHttpActionResult DeleteI_StockTakingItemCatalog(int id)
        {
            I_StockTakingItemCatalog i_StockTakingItemCatalog = db.I_StockTakingItemCatalog.Find(id);
            if (i_StockTakingItemCatalog == null)
            {
                return NotFound();
            }

            db.I_StockTakingItemCatalog.Remove(i_StockTakingItemCatalog);
            db.SaveChanges();

            return Ok(i_StockTakingItemCatalog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool I_StockTakingItemCatalogExists(int id)
        {
            return db.I_StockTakingItemCatalog.Count(e => e.Id == id) > 0;
        }
    }
}