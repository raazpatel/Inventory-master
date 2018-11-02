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

namespace InventoryPizzaExpress.Controllers.API.Store
{
    public class StoresController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/Stores
        public List<Store_Details> GetStore_Details()
        {
            return db.Store_Details.ToList();
        }

        public List<Store_Details> GetStore()
        {
            return db.Store_Details.ToList();
        }


        // GET: api/Stores/5
        [ResponseType(typeof(Store_Details))]
        public IHttpActionResult GetStore_Details(int id)
        {
            Store_Details store_Details = db.Store_Details.Find(id);
            if (store_Details == null)
            {
                return NotFound();
            }

            return Ok(store_Details);
        }

        // PUT: api/Stores/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStore_Details(int id, Store_Details store_Details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != store_Details.storeId)
            {
                return BadRequest();
            }

            db.Entry(store_Details).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Store_DetailsExists(id))
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

        // POST: api/Stores
        [ResponseType(typeof(Store_Details))]
        public IHttpActionResult PostStore_Details(Store_Details store_Details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Store_Details.Add(store_Details);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Store_DetailsExists(store_Details.storeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = store_Details.storeId }, store_Details);
        }

        // DELETE: api/Stores/5
        [ResponseType(typeof(Store_Details))]
        public IHttpActionResult DeleteStore_Details(int id)
        {
            Store_Details store_Details = db.Store_Details.Find(id);
            if (store_Details == null)
            {
                return NotFound();
            }

            db.Store_Details.Remove(store_Details);
            db.SaveChanges();

            return Ok(store_Details);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Store_DetailsExists(int id)
        {
            return db.Store_Details.Count(e => e.storeId == id) > 0;
        }
    }
}