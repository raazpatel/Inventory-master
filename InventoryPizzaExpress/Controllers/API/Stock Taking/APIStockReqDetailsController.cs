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
    public class APIStockReqDetailsController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/APIStockReqDetails
        public IQueryable<I_StockReqDetails> GetI_StockReqDetails()
        {
            return db.I_StockReqDetails;
        }

        // GET: api/APIStockReqDetails/5
        [ResponseType(typeof(I_StockReqDetails))]
        public IHttpActionResult GetI_StockReqDetails(int id)
        {
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);
            if (i_StockReqDetails == null)
            {
                return NotFound();
            }

            return Ok(i_StockReqDetails);
        }

        // PUT: api/APIStockReqDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_StockReqDetails(int id, I_StockReqDetails i_StockReqDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_StockReqDetails.Id)
            {
                return BadRequest();
            }

            db.Entry(i_StockReqDetails).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_StockReqDetailsExists(id))
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

        // POST: api/APIStockReqDetails
        [ResponseType(typeof(I_StockReqDetails))]
        public IHttpActionResult PostI_StockReqDetails(I_StockReqDetails i_StockReqDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_StockReqDetails.Add(i_StockReqDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_StockReqDetails.Id }, i_StockReqDetails);
        }

        [ResponseType(typeof(I_StockRequestItemCatalog))]
        public IHttpActionResult PostI_StockRequestItemCatalog(List<I_StockRequestItemCatalog> i_StockReqDetails)
        {

            foreach (I_StockRequestItemCatalog i_orderMaster in i_StockReqDetails)
            {

                I_StockReqDetails i_OrderDetails = new I_StockReqDetails();
                i_OrderDetails = db.I_StockReqDetails.Find(i_orderMaster.ReqNo);
                i_OrderDetails.Status = 2;             
                db.Entry(i_OrderDetails).State = EntityState.Modified;
                db.SaveChanges();
                break;
            }

            foreach (I_StockRequestItemCatalog item in i_StockReqDetails)
            {
                if (I_stockExists(Convert.ToInt32(item.ItemCode), item.ReqNo))
                {
                    I_StockRequestItemCatalog i_orderMaster1 = db.I_StockRequestItemCatalog.Where(e => e.ItemCode == item.ItemCode && e.ReqNo == item.ReqNo).FirstOrDefault();
                    if (i_orderMaster1 != null)
                    {
                        db.I_StockRequestItemCatalog.Remove(i_orderMaster1);
                        db.SaveChanges();
                    }

                    db.I_StockRequestItemCatalog.Add(item);
                    db.SaveChanges();
                }
                else
                {
                    db.I_StockRequestItemCatalog.Add(item);
                    db.SaveChanges();
                }             
            }        

            return CreatedAtRoute("DefaultApi", new { }, i_StockReqDetails);
        }
        [ResponseType(typeof(I_StockRequestItemCatalog))]
        public IHttpActionResult PostI_StockTransferItemCatalog(List<I_StockTransferItemCatalog> i_StockReqDetails)
        {
            int ReqNo = 0;
            int orderId = 0;

            foreach (I_StockTransferItemCatalog i_orderMaster in i_StockReqDetails)
            {

                I_StockTranferDetails i_OrderDetails = new I_StockTranferDetails();
                ReqNo = i_orderMaster.ReqNo;
                i_OrderDetails = db.I_StockTranferDetails.Find(i_orderMaster.ReqNo);
                i_OrderDetails.Status = 2;
                db.Entry(i_OrderDetails).State = EntityState.Modified;
                db.SaveChanges();             
               
                I_OrderDetails objOrd = new I_OrderDetails();
                objOrd.DeliveryDate = i_OrderDetails.Date;
                objOrd.Status = 1;
                objOrd.OrderNo = "TRA";
                objOrd.StoreId = i_OrderDetails.TragetStore;
                objOrd.TranferNo = i_OrderDetails.TranferNo;
                objOrd.Reference = "Created against Transfer No " + i_OrderDetails.TranferNo;
                objOrd.CreatedBy = i_OrderDetails.CreatedBy;
                objOrd.CreatedOn = i_OrderDetails.CreatedOn;
                db.I_OrderDetails.Add(objOrd);
                db.SaveChanges();
                orderId = objOrd.Id;
                  I_OrderDetails i_OrderDetail = db.I_OrderDetails.Find(objOrd.Id);
                i_OrderDetail.OrderNo = "ORD" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month + "/" + i_OrderDetails.Id;           
                db.Entry(i_OrderDetail).State = EntityState.Modified;
                db.SaveChanges();


                break;
            }

            foreach (I_StockTransferItemCatalog item in i_StockReqDetails)
            {
                if (I_stockTransExists(Convert.ToInt32(item.ItemCode), item.ReqNo))
                {
                    I_StockTransferItemCatalog i_orderMaster1 = db.I_StockTransferItemCatalog.Where(e => e.ItemCode == item.ItemCode && e.ReqNo == item.ReqNo).FirstOrDefault();
                    if (i_orderMaster1 != null)
                    {
                        db.I_StockTransferItemCatalog.Remove(i_orderMaster1);
                        db.SaveChanges();
                    }

                    db.I_StockTransferItemCatalog.Add(item);
                    db.SaveChanges();

                    I_OrderItemCatalog obj = new I_OrderItemCatalog();
                    obj.ItemName = item.ItemName;
                    obj.ItemsId = item.ItemCode;
                    obj.Qty = item.Qty;
                    obj.Status = 1;
                    obj.Price = db.I_StockInventory.Where(x=>x.ItemId == item.ItemCode).Average(i => i.Price);
                    obj.OrderId = orderId;
                    obj.TotalSum = obj.Price * obj.Qty;                 
                    obj.Unit = item.UnitId;
                    obj.Vendor = 0;
                    obj.FixedorVariable = db.I_InventoryItemMaster.Where(x => x.ItemCode == item.ItemCode).Select(i => i.FixedorVariable).SingleOrDefault(); ;
                    obj.Createdby = item.CreatedBy;
                    obj.CreatedOn = item.CreatedOn;
                    
                    db.I_OrderItemCatalog.Add(obj);
                    db.SaveChanges();

                }
                else
                {
                    db.I_StockTransferItemCatalog.Add(item);
                    db.SaveChanges();

                    I_OrderItemCatalog obj = new I_OrderItemCatalog();
                    obj.ItemName = item.ItemName;
                    obj.ItemsId = item.ItemCode;
                    obj.Qty = item.Qty;
                    obj.Status = 1;
                    obj.Price = db.I_StockInventory.Where(x => x.ItemId == item.ItemCode).Average(i => i.Price);
                    obj.OrderId = orderId;
                    obj.TotalSum = obj.Price * obj.Qty;
                    obj.Unit = item.UnitId;
                    obj.Vendor = 0;
                    obj.FixedorVariable = db.I_InventoryItemMaster.Where(x => x.Id == item.ItemCode).Select(i => i.FixedorVariable).SingleOrDefault(); ;
                    obj.Createdby = item.CreatedBy;
                    obj.CreatedOn = item.CreatedOn;
                    db.I_OrderItemCatalog.Add(obj);
                    db.SaveChanges();
                }   
            }
           

          




            return CreatedAtRoute("DefaultApi", new { }, i_StockReqDetails);
        }
        private bool I_stockExists(int Itemid, int ReqNo)
        {
            return db.I_StockRequestItemCatalog.Count(e => e.ReqNo == ReqNo && e.ItemCode == Itemid) > 0;
        }
        private bool I_stockTransExists(int Itemid, int ReqNo)
        {
            return db.I_StockTransferItemCatalog.Count(e => e.ReqNo == ReqNo && e.ItemCode == Itemid) > 0;
        }
        // DELETE: api/APIStockReqDetails/5
        [ResponseType(typeof(I_StockReqDetails))]
        public IHttpActionResult DeleteI_StockReqDetails(int id)
        {
            I_StockReqDetails i_StockReqDetails = db.I_StockReqDetails.Find(id);
            if (i_StockReqDetails == null)
            {
                return NotFound();
            }

            db.I_StockReqDetails.Remove(i_StockReqDetails);
            db.SaveChanges();

            return Ok(i_StockReqDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool I_StockReqDetailsExists(int id)
        {
            return db.I_StockReqDetails.Count(e => e.Id == id) > 0;
        }
    }
}