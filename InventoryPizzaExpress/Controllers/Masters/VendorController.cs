using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InventoryPizzaExpress.Models.Vendor;
using AutoMapper;

namespace InventoryPizzaExpress.Controllers.Masters
{
    [Authorize]
    public class VendorController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();
        HttpClient client;
        string url = string.Empty;
          string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public VendorController()
        {
            client = new HttpClient();
        
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Vendor
        public async Task<ActionResult> Index()
        {  //The URL of the WEB API Service
            url = uri + "/api/Vendor/GetVendor";
            IEnumerable<I_VenderMaster> vendors = null;
            IEnumerable<Vendor> vendordetails = null;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<I_VenderMaster>>(responseData);
              
                vendordetails = Mapper.Map<IEnumerable<I_VenderMaster>, IEnumerable<Vendor>>(vendors);

            }
            else
            {
                vendordetails = Enumerable.Empty<Vendor>();

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(vendordetails);

        }

        // GET: Vendor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_VenderMaster i_VenderMaster = db.I_VenderMaster.Find(id);
            if (i_VenderMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_VenderMaster);
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Vendor i_VenderMaster)
        {
            url = uri + "/api/Vendor/PostVender";
            client.BaseAddress = new Uri(url);
            i_VenderMaster.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_VenderMaster.CreatedOn = System.DateTime.Now;
            I_VenderMaster vendordetails = Mapper.Map<Vendor, I_VenderMaster>(i_VenderMaster);
            
           

                HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, vendordetails);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                  ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                       

            return View(i_VenderMaster);
        }

        // GET: Vendor/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            url = uri + "/api/Vendor/GetVendor/" + id;
            I_VenderMaster vendors = null;
            Vendor vendordetails = null;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<I_VenderMaster>(responseData);

                vendordetails = Mapper.Map<I_VenderMaster, Vendor>(vendors);
                if (vendordetails == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
               // vendordetails = Enumerable.Empty<Vendor>();

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(vendordetails);

           
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id,  Vendor i_VenderMaster)
        {
            if (ModelState.IsValid)
            {
                i_VenderMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_VenderMaster.ModifiedOn = System.DateTime.Now;
                url = uri + "/api/Vendor/PutVender";
                client.BaseAddress = new Uri(url);
                I_VenderMaster vendordetails = Mapper.Map<Vendor, I_VenderMaster>(i_VenderMaster);
                HttpResponseMessage responseMessage = await client.PutAsJsonAsync(url + "/" + id, vendordetails);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Error");
                //db.Entry(i_VenderMaster).State = EntityState.Modified;
                //db.SaveChanges();

               // return RedirectToAction("Index");
            }
            return View(i_VenderMaster);
        }

        // GET: Vendor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_VenderMaster i_VenderMaster = db.I_VenderMaster.Find(id);
            if (i_VenderMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_VenderMaster);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_VenderMaster i_VenderMaster = db.I_VenderMaster.Find(id);
            db.I_VenderMaster.Remove(i_VenderMaster);
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
