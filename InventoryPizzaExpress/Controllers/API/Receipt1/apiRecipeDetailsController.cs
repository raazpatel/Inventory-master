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

namespace InventoryPizzaExpress.Controllers.API.Receipt
{
    public class apiRecipeDetailsController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/apiRecipeDetails
        public IQueryable<I_RecipeDetails> GetI_RecipeDetails()
        {
            return db.I_RecipeDetails;
        }

        // GET: api/apiRecipeDetails/5
        [ResponseType(typeof(I_RecipeDetails))]
        public IHttpActionResult GetI_RecipeDetails(int id)
        {
            I_RecipeDetails i_RecipeDetails = db.I_RecipeDetails.Find(id);
            if (i_RecipeDetails == null)
            {
                return NotFound();
            }

            return Ok(i_RecipeDetails);
        }

        // PUT: api/apiRecipeDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_RecipeDetails(int id, I_RecipeDetails i_RecipeDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_RecipeDetails.Id)
            {
                return BadRequest();
            }

            db.Entry(i_RecipeDetails).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_RecipeDetailsExists(id))
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

        // POST: api/apiRecipeDetails
        [ResponseType(typeof(I_RecipeDetails))]
        public IHttpActionResult PostI_RecipeDetails(I_RecipeDetails i_RecipeDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_RecipeDetails.Add(i_RecipeDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_RecipeDetails.Id }, i_RecipeDetails);
        }

        // DELETE: api/apiRecipeDetails/5
        [ResponseType(typeof(I_RecipeDetails))]
        public IHttpActionResult DeleteI_RecipeDetails(int id)
        {
            I_RecipeDetails i_RecipeDetails = db.I_RecipeDetails.Find(id);
            if (i_RecipeDetails == null)
            {
                return NotFound();
            }

            db.I_RecipeDetails.Remove(i_RecipeDetails);
            db.SaveChanges();

            return Ok(i_RecipeDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool I_RecipeDetailsExists(int id)
        {
            return db.I_RecipeDetails.Count(e => e.Id == id) > 0;
        }
    }
}