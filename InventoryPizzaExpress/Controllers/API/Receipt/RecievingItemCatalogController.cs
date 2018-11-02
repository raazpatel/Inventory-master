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

namespace InventoryPizzaExpress.Controllers.Receipt
{
    public class RecievingItemCatalogController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/RecievingItemCatalog
        public IQueryable<I_RecievingItemCatalog> GetI_RecievingItemCatalog()
        {
            return db.I_RecievingItemCatalog;
        }

        // GET: api/RecievingItemCatalog/5
        [ResponseType(typeof(I_RecievingItemCatalog))]
        public IHttpActionResult GetI_RecievingItemCatalog(int id)
        {
            I_RecievingItemCatalog i_RecievingItemCatalog = db.I_RecievingItemCatalog.Find(id);
            if (i_RecievingItemCatalog == null)
            {
                return NotFound();
            }

            return Ok(i_RecievingItemCatalog);
        }

        // PUT: api/RecievingItemCatalog/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_RecievingItemCatalog(int id, I_RecievingItemCatalog i_RecievingItemCatalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_RecievingItemCatalog.Id)
            {
                return BadRequest();
            }

            db.Entry(i_RecievingItemCatalog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_RecievingItemCatalogExists(id))
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

        // POST: api/RecievingItemCatalog
        [ResponseType(typeof(I_RecievingItemCatalog))]
        public IHttpActionResult Post(I_RecievingItemCatalog i_RecievingItemCatalog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] arr = i_RecievingItemCatalog.OrderIds.Split(',');

            foreach (var item in arr)
            {
                I_OrderDetails obj = new I_OrderDetails();

                obj = db.I_OrderDetails.Where(x => x.OrderNo == item).SingleOrDefault();
                obj.Status = 2;// 2 for Choose for Recieving pending
                db.Entry(obj).State = EntityState.Modified;              
                db.SaveChanges();               
            }

            db.I_RecievingItemCatalog.Add(i_RecievingItemCatalog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_RecievingItemCatalog.Id }, i_RecievingItemCatalog);
        }

        // POST: api/RecievingItemCatalog
        [ResponseType(typeof(I_StockInventory))]
        public IHttpActionResult PostItems(List<I_StockInventory> i_stockinventory)
        {
            foreach (I_StockInventory item in i_stockinventory)
            {
                db.I_StockInventory.Add(item);
                db.SaveChanges();
            }


            foreach ( I_StockInventory item in i_stockinventory)
            {
                var countStock = db.I_StockInventory.Where(x => x.OrderNo == item.OrderNo).Count();
                var countOrder = db.I_OrderItemCatalog.Where(x => x.I_OrderDetails.OrderNo == item.OrderNo).Count();

                if (countStock == countOrder)
                {
                    I_OrderDetails obj = new I_OrderDetails();

                    obj = db.I_OrderDetails.Where(x => x.OrderNo == item.OrderNo).SingleOrDefault();
                    obj.Status = 3;// 3 for Choose for Recieving completed
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
          




            return CreatedAtRoute("DefaultApi", new {  }, i_stockinventory);
        }

        // DELETE: api/RecievingItemCatalog/5
        [ResponseType(typeof(I_RecievingItemCatalog))]
        public IHttpActionResult DeleteI_RecievingItemCatalog(int id)
        {
            I_RecievingItemCatalog i_RecievingItemCatalog = db.I_RecievingItemCatalog.Find(id);
            if (i_RecievingItemCatalog == null)
            {
                return NotFound();
            }

            db.I_RecievingItemCatalog.Remove(i_RecievingItemCatalog);
            db.SaveChanges();

            return Ok(i_RecievingItemCatalog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool I_RecievingItemCatalogExists(int id)
        {
            return db.I_RecievingItemCatalog.Count(e => e.Id == id) > 0;
        }
    }
}