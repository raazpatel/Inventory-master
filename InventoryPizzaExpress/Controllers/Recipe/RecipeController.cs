using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models;

namespace InventoryPizzaExpress.Controllers.Recipe
{
    [Authorize]
    public class RecipeController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: Recipe
        public ActionResult Index()
        {
            var i_RecipeDetails = db.I_Recipe.Where(x=>x.Status==1);
            return View(i_RecipeDetails.ToList());
        }
        [HttpPost]
        public JsonResult AjaxMethod(string type, int value)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (value == 0)
            {
                items = (from m in db.I_InventoryItemMaster select new SelectListItem { Value = m.Id.ToString(), Text = m.ItemName }).ToList();
            }
            if (value == 1)
            {
                items = (from m in db.I_Recipe join n in db.I_RecipeDetails on m.Id equals n.RecipeId where m.Status == 1 && n.ProdArt==null select new SelectListItem { Value = m.Id.ToString(), Text = m.RecipeName }).ToList();
            }
            return Json(items);
        }
        class Values
        {
            public decimal? Price { get; set; }
            public string Unit { get; set; }
            public int UnitId { get; set; }

        }
        [HttpPost]
        public JsonResult CostAndUnit(string type, int value, int selection)
        {
            List<Values> items = new List<Values>();
            if (selection == 0)
            {
                items = (from m in db.I_InventoryItemMaster  where m.Id == value select new Values { Price = m.Price, Unit = m.I_UnitMaster.UnitName,UnitId=m.I_UnitMaster.Id }).ToList();

            }
            if (selection == 1)
            {

                items = (from m in db.I_RecipeDetails
                         where m.RecipeId == value
                         select new Values
                         {
                             Price = (from logs in db.I_RecipeDetails
                                      where (logs.I_Recipe.Id == value)
                                      select logs.I_InventoryItemMaster.Price * logs.Qty).Sum(),
                             Unit = m.I_InventoryItemMaster.I_UnitMaster.UnitName,
                             UnitId = m.I_UnitMaster.Id
                         }).ToList();

            }
            return Json(items);
        }
        public ActionResult PartialIndex(I_RecipeDetails i_RecipeDetails)
        {

            return RedirectToAction("Create", new
            {
                Rid = i_RecipeDetails.RecipeId,
                invItem = 0
            });
        }

        // GET: Recipe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_RecipeDetails i_RecipeDetails = db.I_RecipeDetails.Find(id);
            if (i_RecipeDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = db.I_RecipeDetails.Include(i => i.I_InventoryItemMaster).Include(i => i.I_MenuItems).Include(i => i.I_Recipe);
            return View(i_RecipeDetails);
        }
        public ActionResult CreateRecipe()
        {
            I_Recipe i_Recipe = new I_Recipe();
            i_Recipe.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_Recipe.CreatedOn = System.DateTime.Now;
            return View(i_Recipe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRecipe(I_Recipe i_Recipe)
        {
            if (ModelState.IsValid)
            {
                i_Recipe.Status = 1;
                db.I_Recipe.Add(i_Recipe);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: Recipe/Create
        public ActionResult Create(int? Rid, int? invItem)
        {

            if (Rid == null)
            {
                return RedirectToAction("Index");
            }
            else if (Rid <= 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Action = "";
            I_RecipeDetails i_RecipeDetails = new I_RecipeDetails();
            ViewBag.Article = new List<SelectListItem> {
                 new SelectListItem { Text = "P", Value = "1" },
                 new SelectListItem { Text = "M", Value = "0"}
                };

            ViewBag.ItemId = new SelectList(db.I_InventoryItemMaster, "Id", "ItemName");
            ViewBag.MemuItem = new SelectList(db.I_MenuItems, "Id", "Name");
            ViewBag.RecipeId = new SelectList(db.I_Recipe, "Id", "RecipeName", Rid);




            List<RecipeDetails> list = (from x in db.I_RecipeDetails.Include(i => i.I_InventoryItemMaster)
                        .Include(i => i.I_MenuItems).Include(i => i.I_Recipe).Include(i => i.I_UnitMaster).AsEnumerable()
                                        where x.RecipeId == Rid
                                        select new RecipeDetails
                                        {
                                            Id = x.Id,
                                            Ingedient = x.Article == 0 ? x.I_InventoryItemMaster.ItemName : (from logs in db.I_Recipe
                                                                                                             where (logs.Id == x.ProdArt)
                                                                                                             select logs.RecipeName).SingleOrDefault(),
                                            Article = x.Article == 1 ? "Prod article" : "Ingredient",
                                            Qty = Convert.ToDecimal(x.Qty),
                                            cost = Math.Round(decimal.Parse(x.Article == 0 ? (x.I_InventoryItemMaster.Price.ToString()) : (from logs in db.I_RecipeDetails
                                                                                                                                           where (logs.I_Recipe.Id == x.ProdArt)
                                                                                                                                           select logs.I_InventoryItemMaster==null?0: logs.I_InventoryItemMaster.Price * logs.Qty).Sum().ToString()), 2).ToString() ,
                                            CreatedBy = x.CreatedBy,
                                            CreatedOn = x.CreatedOn,
                                            IngedientId = x.ProdArt.ToString(),
                                           // ModifiedBy = x.ModifiedBy,
                                           // ModifiedOn = x.ModifiedOn,
                                           // MenuName = x.I_MenuItems.Name,
                                            RecipeId = x.RecipeId,
                                            Unit = x.Unit !=null? x.I_InventoryItemMaster.I_UnitMaster.UnitName:"",
                                        }).ToList();

                        var list1 = from c in db.I_Recipe

                        where c.Id == 1

                        select new
                        {
                            Name = c.RecipeName,
                        };


            ViewBag.userdetails = list;

            
            //  ViewBag.userdetails = db.I_RecipeDetails.Where(x => x.Article == 0).Include(i => i.I_InventoryItemMaster).Include(i => i.I_MenuItems).Include(i => i.I_Recipe).Where(d => d.RecipeId == Rid).ToList();
            ViewBag.Cost = list.Sum(x =>(Convert.ToDecimal(  x.cost==""?"0": x.cost) * x.Qty)).ToString();
            if (invItem == null)
            {
                i_RecipeDetails.RecipeId = Rid;
                i_RecipeDetails.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_RecipeDetails.CreatedOn = System.DateTime.Now;
                return View(i_RecipeDetails);
            }
            else if (invItem > 0)
            {
                ViewBag.Action = "Edit";
            }
            i_RecipeDetails = db.I_RecipeDetails.Find(invItem);
            if (i_RecipeDetails == null)
            {
                ViewBag.Action = "Create";
                I_RecipeDetails RecipeDetails = new I_RecipeDetails();
                RecipeDetails.RecipeId = Rid;
                RecipeDetails.CreatedBy = System.Web.HttpContext.Current.User.Identity.Name;
                RecipeDetails.CreatedOn = System.DateTime.Now;
                return View(RecipeDetails);
            }



            if (i_RecipeDetails.Article == 1)
            {

                ViewBag.Price = Convert.ToDecimal((from logs in list
                                                                                 where (logs.IngedientId == i_RecipeDetails.ProdArt.ToString())
                                                                                 select logs.cost).SingleOrDefault());
            }
            else
            {
                i_RecipeDetails.I_InventoryItemMaster.Price = Convert.ToDecimal((from logs in list
                                                                                 where (logs.Id == invItem)
                                                                                 select logs.cost).SingleOrDefault());
            }
            if (i_RecipeDetails.Article == 0)
            {
                ViewBag.ItemId = new SelectList(db.I_InventoryItemMaster, "Id", "ItemName", i_RecipeDetails.ItemId);
               
            }
            if (i_RecipeDetails.Article == 1)
            {
                ViewBag.ItemId = new SelectList(db.I_Recipe, "Id", "RecipeName", i_RecipeDetails.ItemId);
              
            }
          //  ViewBag.ItemId = new SelectList(db.I_InventoryItemMaster, "Id", "ItemName", i_RecipeDetails.ItemId);
            ViewBag.MemuItem = new SelectList(db.I_MenuItems, "Id", "Name", i_RecipeDetails.MemuItem);
            ViewBag.RecipeId = new SelectList(db.I_Recipe, "Id", "RecipeName", i_RecipeDetails.RecipeId);

            return View(i_RecipeDetails);



        }



        // POST: Recipe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MemuItem,RecipeId,Article,ItemId,Qty,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_RecipeDetails i_RecipeDetails )
        {

            if (ModelState.IsValid)
            {
                i_RecipeDetails.Unit = (from m in db.I_InventoryItemMaster where m.Id == i_RecipeDetails.ItemId select m.I_UnitMaster.Id).SingleOrDefault();
                //i_UnitMasters.Id = (from m in db.I_RecipeDetails );

                if (i_RecipeDetails.Article == 1)
                {
                    i_RecipeDetails.ProdArt = i_RecipeDetails.ItemId;
                    //i_RecipeDetails.ItemId = null;
                    //i_RecipeDetails.Unit = null;
                    i_RecipeDetails.ItemId = i_RecipeDetails.ItemId;
                    i_RecipeDetails.Unit = i_RecipeDetails.Unit;
                }

                db.I_RecipeDetails.Add(i_RecipeDetails);
                db.SaveChanges();
                return RedirectToAction("Create", new
                {
                    Rid = i_RecipeDetails.RecipeId,
                    invItem = 0
                });
            }

            ViewBag.ItemId = new SelectList(db.I_InventoryItemMaster, "Id", "ItemName", i_RecipeDetails.ItemId);
            ViewBag.MemuItem = new SelectList(db.I_MenuItems, "Id", "Name", i_RecipeDetails.MemuItem);
            ViewBag.RecipeId = new SelectList(db.I_Recipe, "Id", "RecipeName", i_RecipeDetails.RecipeId);

            return RedirectToAction("Create", new
            {
                Rid = i_RecipeDetails.RecipeId,
                invItem = 0
            });
            // return View(i_RecipeDetails);
        }

        // GET: Recipe/Edit/5
        public ActionResult EditRecipe(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_Recipe i_Recipe = db.I_Recipe.Find(id);
            if (i_Recipe == null)
            {
                return HttpNotFound();
            }
            return View(i_Recipe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecipe(I_Recipe i_Recipe)
        {

            if (ModelState.IsValid)
            {
                i_Recipe.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_Recipe.ModifiedOn = System.DateTime.Now;
                i_Recipe.Status = 1;
                db.Entry(i_Recipe).State = EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemuItem,RecipeId,Article,ItemId,Qty,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] I_RecipeDetails i_RecipeDetails)
        {
            if (ModelState.IsValid)
            {
                i_RecipeDetails.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                i_RecipeDetails.ModifiedOn = System.DateTime.Now;

                if (i_RecipeDetails.Article == 1)
                {                  
                    i_RecipeDetails.Unit = null;
                    i_RecipeDetails.ProdArt = i_RecipeDetails.ItemId;
                    i_RecipeDetails.ItemId = null;
                }
                else
                {
                    i_RecipeDetails.Unit = (from m in db.I_InventoryItemMaster where m.Id == i_RecipeDetails.ItemId select m.I_UnitMaster.Id).SingleOrDefault();
                }
              
                db.Entry(i_RecipeDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", new
                {
                    Rid = i_RecipeDetails.RecipeId,
                    invItem = 0
                });
            }
            ViewBag.ItemId = new SelectList(db.I_InventoryItemMaster, "Id", "ItemName", i_RecipeDetails.ItemId);
            ViewBag.MemuItem = new SelectList(db.I_MenuItems, "Id", "Name", i_RecipeDetails.MemuItem);
            ViewBag.RecipeId = new SelectList(db.I_Recipe, "Id", "RecipeName", i_RecipeDetails.RecipeId);
            return RedirectToAction("Create", new
            {
                Rid = i_RecipeDetails.RecipeId,
                invItem = 0
            });
        }

        public ActionResult DeleteRecipe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_Recipe i_Recipe = db.I_Recipe.Find(id);
            if (i_Recipe == null)
            {
                return HttpNotFound();
            }
            i_Recipe.Status = -99;
            i_Recipe.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            i_Recipe.ModifiedOn = System.DateTime.Now;
            db.Entry(i_Recipe).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Recipe/Delete/5
        public ActionResult Delete(int? Rid)
        {
            if (Rid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            I_RecipeDetails i_RecipeDetails = db.I_RecipeDetails.Find(Rid);
            if (i_RecipeDetails == null)
            {
                return HttpNotFound();
            }

            db.I_RecipeDetails.Remove(i_RecipeDetails);
            db.SaveChanges();
            return RedirectToAction("Create", new
            {
                Rid = i_RecipeDetails.RecipeId,
                invItem = 0
            });
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
