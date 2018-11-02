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
using InventoryPizzaExpress.Models.Order;
using System.Globalization;

namespace InventoryPizzaExpress.Controllers.API.Order
{
    public class OrderDetailsController : ApiController
    {
        private InventoryModuleEntities db = new InventoryModuleEntities();

        // GET: api/OrderDetails
        public IQueryable<I_OrderDetails> GetI_OrderDetails()
        {
            return db.I_OrderDetails;
        }

        public IList<FinalOrder> GetFinalOrderedItems()
        {
            IEnumerable<I_VenderMaster> vendor = db.I_VenderMaster;
            List<FinalOrder> i_OrderDetails = db.I_OrderDetails.Include(i => i.Store_Details).Where(x => x.Status == 1).Select(x => new FinalOrder()
            {
                OrderId = x.Id.ToString(),
                StoreId = x.StoreId.ToString(),
                StoreName = x.Store_Details.storename,
                Reference = x.Reference,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                DeliveryDate = x.DeliveryDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                OrderName = x.OrderNo,
                Total = (from logs in db.I_OrderItemCatalog
                         where (logs.OrderId == x.Id)
                         select logs.Price * logs.Qty).Sum().ToString(),
                Vendor = (from v in vendor where v.Id.ToString() == x.VendorId.ToString() select v.VenderName).FirstOrDefault(),
               
            }).ToList();
            return i_OrderDetails;
        }

        public IList<FinalOrder> GetSearchOrderedItems(string storeId, string vendorId ,string Date)
        {

            List<FinalOrder> i_OrderDetails = db.I_OrderDetails.Include(i => i.Store_Details).Where(x => x.Status == 1).Select(x => new FinalOrder()
            {
                OrderId = x.Id.ToString(),
                StoreId = x.StoreId.ToString(),
                StoreName = x.Store_Details.storename,
                Reference = x.Reference,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                DeliveryDate = x.DeliveryDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                OrderName = x.OrderNo,
                Total = (from logs in db.I_OrderItemCatalog
                         where (logs.OrderId == x.Id)
                         select logs.Price * logs.Qty).Sum().ToString(),
               // Vendor = x.I_VenderMaster.VenderName,
                VendorId = x.VendorId.ToString()
            }).ToList();
            if (storeId != "0")
            {
                i_OrderDetails = i_OrderDetails.Where(r => r.StoreId == storeId).ToList();
            }
            if (vendorId != "0")
            {
                i_OrderDetails = i_OrderDetails.Where(r => r.VendorId == vendorId).ToList();
            }
            if (Date != "")
            {
                i_OrderDetails = i_OrderDetails.Where(r => r.DeliveryDate.ToString("MM/dd/yyyy") == (Date)).ToList();
            }
            return i_OrderDetails;
           
        }
        public IList<OrdersCustom> GetOrderedItemsforReceiving(string orderIds)
        {


            IEnumerable<I_VenderMaster> vendor = db.I_VenderMaster;
            string[] arr = orderIds.Split(',');

            List<OrdersCustom> i_OrderDetails = db.I_OrderItemCatalog.Include(i => i.I_OrderDetails).Where(x => x.Status == 1 && arr.Contains(x.I_OrderDetails.OrderNo)).Select(x => new OrdersCustom()
            {
                OrderId = x.I_OrderDetails.Id,
                ItemsId = x.ItemsId,
                ItemName = x.ItemName,
                Qty = x.Qty,
                Price = x.Price,
                CreatedOn = x.CreatedOn,
                Createdby = x.Createdby,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                OrderName = x.I_OrderDetails.OrderNo,
                //VendorName = x.I_VenderMaster.VenderName,
                VendorName = (from v in vendor where v.Id.ToString() == x.Vendor.ToString() select v.VenderName).FirstOrDefault(),
                VendorId = x.Vendor,
                Unit = x.Unit,
                UnitName = x.I_UnitMaster.UnitName,
                Storeid = x.I_OrderDetails.StoreId.ToString(),
                StoreName =x.I_OrderDetails.Store_Details.storename,
                Status = db.I_StockInventory.Where(y => y.OrderNo == x.I_OrderDetails.OrderNo && y.ItemId == x.ItemsId).Count() > 0 ? 1 : 0


            }).ToList();
          
            return i_OrderDetails;

        }

        [ResponseType(typeof(I_OrderItemCatalog))]
        public IHttpActionResult PostOrderItems(List<I_OrderItemCatalog> i_orderMasters)
        {
            try
            {
                foreach (I_OrderItemCatalog i_orderMaster in i_orderMasters)
                {

                    I_OrderDetails i_OrderDetails = new I_OrderDetails();
                    i_OrderDetails = db.I_OrderDetails.Find(i_orderMaster.OrderId);
                    i_OrderDetails.Status = i_orderMaster.Status;
                    i_OrderDetails.VendorId = i_orderMaster.Vendor;
                    db.Entry(i_OrderDetails).State = EntityState.Modified;
                     db.SaveChanges();
                    break;
                }
                foreach (I_OrderItemCatalog i_orderMaster in i_orderMasters)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (I_OrderExists(Convert.ToInt32(i_orderMaster.ItemsId), i_orderMaster.OrderId))
                    {// Status =1 Ordered
                     //Status = 0 Saved
                     //Status =-99 Deleted
                        I_OrderItemCatalog i_orderMaster1 = db.I_OrderItemCatalog.Where(e => e.ItemsId == i_orderMaster.ItemsId && e.OrderId == i_orderMaster.OrderId && e.Status != -99).FirstOrDefault();
                        if (i_orderMaster1 != null)
                        {
                            db.I_OrderItemCatalog.Remove(i_orderMaster1);
                            db.SaveChanges();
                        }

                        db.I_OrderItemCatalog.Add(i_orderMaster);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.I_OrderItemCatalog.Add(i_orderMaster);
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
           
          
            return Ok();
        }
        // GET: api/OrderDetails/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult GetI_OrderDetails(int id)
        {
            I_StockInventory obj = new I_StockInventory();
            IEnumerable<I_VenderMaster> vendor = db.I_VenderMaster;
            List<Orders> i_OrderDetailsNew = new List<Orders>();
            List<Orders> i_OrderDetails = db.I_OrderItemCatalog.Where(X => X.OrderId == id).Select(x => new Orders()
            {
                Id = x.Id,
                OrderId = x.OrderId,
                OrderName = x.I_OrderDetails.OrderNo,
                ItemsId = x.ItemsId,
                ItemName = x.ItemName,
                Price = x.Price,
                onHand = x.onHand,
                onOrder = x.onOrder,
                Qty = x.Qty,
                Unit = x.Unit,
                UnitName = x.I_UnitMaster.UnitName,
                VendorName = (from v in vendor where v.Id.ToString() == x.Vendor.ToString() select v.VenderName).FirstOrDefault(),
                VendorId = x.Vendor,
                Status = x.Status,
                TotalSum = x.TotalSum,
                Createdby = x.Createdby,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                FixedorVariable=x.FixedorVariable,

            }).ToList();




            if ((i_OrderDetails == null) && (i_OrderDetails.Count == 0))
            {
                return NotFound();
            }

            return Ok(i_OrderDetails);
        }

        // PUT: api/OrderDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutI_OrderDetails(int id, I_OrderDetails i_OrderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != i_OrderDetails.Id)
            {
                return BadRequest();
            }

            db.Entry(i_OrderDetails).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!I_OrderDetailsExists(id))
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

        // POST: api/OrderDetails
        [ResponseType(typeof(I_OrderDetails))]
        public IHttpActionResult PostI_OrderDetails(I_OrderDetails i_OrderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.I_OrderDetails.Add(i_OrderDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = i_OrderDetails.Id }, i_OrderDetails);
        }

        // DELETE: api/OrderDetails/5
        [ResponseType(typeof(I_OrderDetails))]
        public IHttpActionResult DeleteI_OrderDetails(int id)
        {
            I_OrderDetails i_OrderDetails = db.I_OrderDetails.Find(id);
            if (i_OrderDetails == null)
            {
                return NotFound();
            }

            db.I_OrderDetails.Remove(i_OrderDetails);
            db.SaveChanges();

            return Ok(i_OrderDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool I_OrderExists(int Itemid ,int OrderId)
        {
            return db.I_OrderItemCatalog.Count(e => e.OrderId == OrderId && e.ItemsId == Itemid && e.Status!=-99) > 0;
        }
        private bool I_OrderDetailsExists(int id)
        {
            return db.I_OrderDetails.Count(e => e.Id == id) > 0;
        }
    }
}