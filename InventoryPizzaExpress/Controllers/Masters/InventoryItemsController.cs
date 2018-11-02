using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace InventoryPizzaExpress.Controllers
{
    [Authorize]
    public class InventoryItemsController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: InventoryItems
        public ActionResult Index()
        {
            var i_InventoryItemMaster = db.I_InventoryItemMaster.Include(i => i.I_ItemMater).Include(i => i.I_ItemMater1).Include(i => i.I_UnitMaster).Include(i => i.I_UnitMaster1).Include(i => i.I_UnitMaster2).OrderByDescending(i => i.CreatedOn);
            return View(i_InventoryItemMaster.OrderByDescending(i=>i.CreatedOn).ToList());
        }

       
        [HttpPost]
        public JsonResult doesItemNameExist(string ItemName)
        {

            return Json(IsItemAvailable(ItemName));

        }

        public bool IsItemAvailable(string ItemName)
        {
            //if (i_InventoryItemMaster.ItemCode == 0)
            //{
            //    i_InventoryItemMaster.ItemCode = db.I_InventoryItemMaster.Max(x => x.ItemCode) + 1;
            //}
            var RegItem = (from u in db.I_InventoryItemMaster
                              where u.ItemName.ToUpper() == ItemName.ToUpper()
                              select new { ItemName }).FirstOrDefault();

            bool status;
            if (RegItem != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }

            return status;
        }
    


    [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                conString = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                   
                }
            }

            return View();
        }

        // GET: InventoryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_InventoryItemMaster i_InventoryItemMaster = db.I_InventoryItemMaster.Find(id);
            if (i_InventoryItemMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_InventoryItemMaster);
        }

        // GET: InventoryItems/Create
        public ActionResult Create()
        {
            ViewBag.FixedorVariable = new List<SelectListItem> {
                 new SelectListItem { Text = "Fixed", Value = "1" },
                 new SelectListItem { Text = "Variable", Value = "0"}
             };
            I_InventoryItemMaster i_ItemMater = new I_InventoryItemMaster();
            i_ItemMater.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_ItemMater.CreatedOn = System.DateTime.Now;
            ViewBag.MajorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == null && x.Status == true), "Id", "ItemName");
            ViewBag.MinorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == -1 && x.Status == true), "Id", "ItemName");
            ViewBag.PrimaryUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName");
            ViewBag.ReportingUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName");
            ViewBag.StoreUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName");
            return View(i_ItemMater);
        }
        [HttpPost]
        public JsonResult AjaxMethod(string type, int value)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items = (from m in db.I_ItemMater where m.MajorItemId == value && m.Status == true select new SelectListItem { Value = m.Id.ToString(), Text = m.ItemName }).ToList();
            return Json(items);
        }
        // POST: InventoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemCode,ItemName,MajorItemId,MinorItemId,PrimaryUnit,PrimaryUnitConv,ReportingUnit,ReportingUnitConv,StoreUnit,StoreUnitConv,LossPercentage,GainPercentage,ThersoldRecieving,FixedorVariable,Price,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_InventoryItemMaster i_InventoryItemMaster)
        {
            if (ModelState.IsValid)
            {
                if(i_InventoryItemMaster.ItemCode ==0)
                {
                    i_InventoryItemMaster.ItemCode = db.I_InventoryItemMaster.Max(x => x.ItemCode) + 1;
                }
               


                db.I_InventoryItemMaster.Add(i_InventoryItemMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FixedorVariable = new List<SelectListItem> {
                 new SelectListItem { Text = "Fixed", Value = "1" },
                 new SelectListItem { Text = "Variable", Value = "0"}
             };
            ViewBag.MajorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == null && x.Status == true), "Id", "ItemName");
            ViewBag.MinorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == -1 && x.Status == true), "Id", "ItemName");
            ViewBag.PrimaryUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.PrimaryUnit);
            ViewBag.ReportingUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.ReportingUnit);
            ViewBag.StoreUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.StoreUnit);
            return View(i_InventoryItemMaster);
        }

        // GET: InventoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_InventoryItemMaster i_InventoryItemMaster = db.I_InventoryItemMaster.Find(id);
            if (i_InventoryItemMaster == null)
            {
                return HttpNotFound();
            }

            ViewBag.Price = i_InventoryItemMaster.FixedorVariable;
            //ViewBag.FixedorVariable = new List<SelectListItem> {
            //     new SelectListItem { Text = "Fixed", Value = "1" },
            //     new SelectListItem { Text = "Variable", Value = "0"}
            // };
            ViewBag.MajorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == null && x.Status == true), "Id", "ItemName", i_InventoryItemMaster.MajorItemId);
            ViewBag.MinorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId != null && x.Status == true && x.MajorItemId ==  i_InventoryItemMaster.MajorItemId ), "Id", "ItemName", i_InventoryItemMaster.MinorItemId);
            ViewBag.PrimaryUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.PrimaryUnit);
            ViewBag.ReportingUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.ReportingUnit);
            ViewBag.StoreUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.StoreUnit);
            return View(i_InventoryItemMaster);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemCode,ItemName,MajorItemId,MinorItemId,PrimaryUnit,PrimaryUnitConv,ReportingUnit,ReportingUnitConv,StoreUnit,StoreUnitConv,LossPercentage,GainPercentage,ThersoldRecieving,FixedorVariable,Price,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_InventoryItemMaster i_InventoryItemMaster)
        {
            ViewBag.FixedorVariable = new List<SelectListItem> {
                 new SelectListItem { Text = "Fixed", Value = "1" },
                 new SelectListItem { Text = "Variable", Value = "0"}
             };
            if (ModelState.IsValid)
            {
                i_InventoryItemMaster.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_InventoryItemMaster.ModifiedOn = System.DateTime.Now;             
                db.Entry(i_InventoryItemMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MajorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == null && x.Status == true), "Id", "ItemName");
            ViewBag.MinorItemId = new SelectList(db.I_ItemMater.Where(x => x.MajorItemId == -1 && x.Status == true), "Id", "ItemName");
            ViewBag.PrimaryUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.PrimaryUnit);
            ViewBag.ReportingUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.ReportingUnit);
            ViewBag.StoreUnit = new SelectList(db.I_UnitMaster, "Id", "UnitName", i_InventoryItemMaster.StoreUnit);
            return View(i_InventoryItemMaster);
        }

        // GET: InventoryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_InventoryItemMaster i_InventoryItemMaster = db.I_InventoryItemMaster.Find(id);
            if (i_InventoryItemMaster == null)
            {
                return HttpNotFound();
            }
            return View(i_InventoryItemMaster);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            I_InventoryItemMaster i_InventoryItemMaster = db.I_InventoryItemMaster.Find(id);
            db.I_InventoryItemMaster.Remove(i_InventoryItemMaster);
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
