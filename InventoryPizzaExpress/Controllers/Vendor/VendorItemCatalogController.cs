using AutoMapper;
using InventoryPizzaExpress.Models;
using InventoryPizzaExpress.Models.Inventory;
using InventoryPizzaExpress.Models.Vendor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryPizzaExpress.Controllers
{
    [Authorize]
    public class VendorItemCatalogController : Controller
    {

        private InventoryModuleEntities db = new InventoryModuleEntities();
        HttpClient client;
        string url = string.Empty;
        string uri = System.Configuration.ConfigurationManager.AppSettings["APiUrl"].ToString();
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        public VendorItemCatalogController()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult VendorItemCatalog()
        {
           return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetMinorGroups()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            url = uri + "/api/MinorGroup/GetMinorGroups";
            IEnumerable<Items_Major> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<Items_Major>>(responseData);
            }
            items = (from m in vendors select new SelectListItem { Value = m.Id.ToString(), Text = m.ItemName }).ToList();
           
            return Json(items);
        }
        [HttpPost]
        public async Task<JsonResult> GetItems(int value, int vendor)
        {

            url = uri + "/api/MinorGroup/GetItems?id=" + value+ "&vendor="+ vendor;
            IEnumerable<InventoryItems> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<InventoryItems>>(responseData);
            }
           
            return Json(vendors);
        }
        [HttpPost]
        public async Task<JsonResult> GetVendoeItems(int value )
        {

            url = uri + "/api/Vendor/GetVendorItems/" + value ;
            IEnumerable<VendorItemCatalog> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<VendorItemCatalog>>(responseData);
            }

            return Json(vendors);
        }
        [HttpPost]
        public async Task<JsonResult> GetVendoeItemsOrder(int value, int order)
        {

            url = uri + "/api/Vendor/GetVendorItemsOrder?id=" + value+ "&order="+ order;
            IEnumerable<VendorItemCatalog> vendors = null;

            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                vendors = JsonConvert.DeserializeObject<List<VendorItemCatalog>>(responseData);
            }

            return Json(vendors);
        }
        [HttpPost]
        public async Task<JsonResult> GetVendors()
        {
            List<SelectListItem> items = new List<SelectListItem>();
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
            items = (from m in vendordetails select new SelectListItem { Value = m.Id.ToString(), Text = m.VenderName }).ToList();
            return Json(items);
        }
        [HttpPost]      
        public async Task<ActionResult> AddItems(int Id, int itemId, string ItemName,string ItemCode,string ItemPrice,string effectiveDate, string effectiveDateTo,int Unit, int fixedorvariable, int status,int VendorId)
        {
          




            I_VendorItemCatalog obj = new I_VendorItemCatalog();
            obj.Id = (Id);
            obj.ItemId = (itemId);
            obj.ItemName = (ItemName);
            obj.ItemCode = (ItemCode);
            obj.Price = Convert.ToDecimal(ItemPrice);
            obj.DateEffectiveFrom = Convert.ToDateTime(effectiveDate);
            if(effectiveDateTo!="")
            obj.DateEffectiveTo =Convert.ToDateTime(effectiveDateTo);
            obj.Status = (status);
            obj.VendorId = (VendorId);
            obj.Unit = (Unit);
            obj.FixedorVariable = fixedorvariable;
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            url = uri + "/api/Vendor/PostVenderItems";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, obj);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new
                {
                    success = true,
                    flag=1
                },
            JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false
            },
             JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> AddNewItems(int itemId, string ItemName, string ItemCode, string ItemPrice, string effectiveDate, string effectiveDateTo, int Unit, int fixedorvariable, int status, int VendorId)
        {

            I_VendorItemCatalog obj = new I_VendorItemCatalog();
            obj.ItemId = (itemId);
            obj.ItemName = (ItemName);
            obj.ItemCode = (ItemCode);
            obj.Price = Convert.ToDecimal(ItemPrice);
            obj.DateEffectiveFrom = Convert.ToDateTime(effectiveDate);
            if (effectiveDateTo != "")
                obj.DateEffectiveTo = Convert.ToDateTime(effectiveDateTo);
            obj.Status = (status);
            obj.VendorId = (VendorId);
            obj.Unit = (Unit);
            obj.FixedorVariable = fixedorvariable;
            obj.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            obj.CreatedOn = System.DateTime.Now;
            url = uri + "/api/Vendor/PostVenderAddNewItems";
            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(url, obj);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new
                {
                    success = true
                },
            JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false
            },
             JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            I_VendorItemCatalog obj = new I_VendorItemCatalog();
            url = uri + "/api/Vendor/PutRemoveItem/" + id;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage responseMessage = await client.PutAsJsonAsync(url, obj);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new
                {
                    success = true
                },
            JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false
            },
             JsonRequestBehavior.AllowGet);
        }
    }
}