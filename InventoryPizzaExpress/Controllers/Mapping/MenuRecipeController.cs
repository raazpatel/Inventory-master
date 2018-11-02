using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryPizzaExpress;
using InventoryPizzaExpress.Models.Mapping;

namespace InventoryPizzaExpress.Controllers.Mapping
{
    [Authorize]
    public class MenuRecipeController : Controller
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: MenuRecipe
        public ActionResult Index()
        {
            List<SelectListItem> SelectListItem = new List<SelectListItem>();
            SelectListItem = db.I_Recipe.Select(x=> new SelectListItem {
                Text =x.RecipeName,
                Value = x.Id.ToString(),
            }).ToList();

            ViewBag.Recipe = SelectListItem;
            return View(from m in db.mi_def where (m.storeid == 1001) select (new MenuRecipe {
                mi_seq = m.mi_seq,
                v_mi_def_Id = m.mi_def_Id,
                storeid = m.storeid,
                master_item_Id = m.master_item_Id,
                name_1 = m.name_1,
                name_2 = m.name_2,
                obj_num = m.obj_num,
                RecipeId = (from x in db.I_Recipe where (x.Id == m.RecipeId) select x.RecipeName).FirstOrDefault()
            }));
        }
        [HttpPost]
        public JsonResult ManualMapping(int SourcemenuId, int targetmenuId, int storeid)
        {
            string output = "";
            try
            {
                mi_def mi_def = new mi_def();
                mi_def = (from m in db.mi_def where m.obj_num.ToString() == targetmenuId.ToString() && m.storeid == storeid select m).FirstOrDefault();
                mi_def.RecipeId = SourcemenuId;
                db.Entry(mi_def).State = EntityState.Modified;
                db.SaveChanges();
                output = "Ok";
            }
            catch (Exception ex)
            {
                output = "Error";

            }
           
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        // GET: MenuRecipe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mi_def mi_def = db.mi_def.Find(id);
            if (mi_def == null)
            {
                return HttpNotFound();
            }
            return View(mi_def);
        }

        // GET: MenuRecipe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuRecipe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mi_def_Id,storeid,mi_seq,obj_num,name_1,name_2,maj_grp_seq,fam_grp_seq,mi_grp_seq,mi_slu_seq,price_grp_seq,slu_priority,nlu_grp,nlu_num,key_num,icon_id,ob_mi31_chk_mi_avail,ob_mi44_no_edit_in_mgr_proc,ob_item_is_the_no_modifier,ob_lite_mi_dirty,ob_rsvd01,ob_rsvd02,ob_rsvd03,ob_rsvd04,mi_type_seq,cond_grp_mem_seq,cond_req,cond_allowed,crs_mem_seq,crs_sel_seq,mlvl_class_seq,prn_def_class_seq,product_seq_1,product_seq_2,product_seq_3,product_seq_4,comm_amt,comm_pcnt,cross_ref1,cross_ref2,ob_flags,ob_workstation_only,mi_slu2_seq,prep_time,external_type,topping_type_seq,topping_modifier_seq,last_updated_by,last_updated_date,multi_user_access_seq,build_screen_style_seq,hht_build_screen_style_seq,prefix_override_count,prefix_override_level,mi_slu3_seq,mi_slu4_seq,mi_slu5_seq,mi_slu6_seq,mi_slu7_seq,mi_slu8_seq,DateCreated,master_item_Id,RecipeId")] mi_def mi_def)
        {
            if (ModelState.IsValid)
            {
                db.mi_def.Add(mi_def);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mi_def);
        }

        // GET: MenuRecipe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mi_def mi_def = db.mi_def.Find(id);
            if (mi_def == null)
            {
                return HttpNotFound();
            }
            return View(mi_def);
        }

        // POST: MenuRecipe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mi_def_Id,storeid,mi_seq,obj_num,name_1,name_2,maj_grp_seq,fam_grp_seq,mi_grp_seq,mi_slu_seq,price_grp_seq,slu_priority,nlu_grp,nlu_num,key_num,icon_id,ob_mi31_chk_mi_avail,ob_mi44_no_edit_in_mgr_proc,ob_item_is_the_no_modifier,ob_lite_mi_dirty,ob_rsvd01,ob_rsvd02,ob_rsvd03,ob_rsvd04,mi_type_seq,cond_grp_mem_seq,cond_req,cond_allowed,crs_mem_seq,crs_sel_seq,mlvl_class_seq,prn_def_class_seq,product_seq_1,product_seq_2,product_seq_3,product_seq_4,comm_amt,comm_pcnt,cross_ref1,cross_ref2,ob_flags,ob_workstation_only,mi_slu2_seq,prep_time,external_type,topping_type_seq,topping_modifier_seq,last_updated_by,last_updated_date,multi_user_access_seq,build_screen_style_seq,hht_build_screen_style_seq,prefix_override_count,prefix_override_level,mi_slu3_seq,mi_slu4_seq,mi_slu5_seq,mi_slu6_seq,mi_slu7_seq,mi_slu8_seq,DateCreated,master_item_Id,RecipeId")] mi_def mi_def)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mi_def).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mi_def);
        }

        // GET: MenuRecipe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            mi_def mi_def = db.mi_def.Find(id);
            if (mi_def == null)
            {
                return HttpNotFound();
            }
            return View(mi_def);
        }

        // POST: MenuRecipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            mi_def mi_def = db.mi_def.Find(id);
            db.mi_def.Remove(mi_def);
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
