using Hub.Models;
using DatelAPI.Areas.PurchaseOrders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace DatelAPI.Areas.PurchaseOrders.Controllers
{
    [RoutePrefix("sage/purchaseorders")]
    public class PurchaseOrdersController : ApiController
    {
        private readonly IPurchaseOrdersRepository _repository;

        public PurchaseOrdersController(IPurchaseOrdersRepository repository)
        {
            _repository = repository;
        }


        [Route("Save")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateOrder(PurchaseOrder po)
        {

            SageOrderResponse result = await _repository.CreatePurchaseOrder(po);
            return Json(result);
        }

        [Route("Test")]
        [HttpGet]
        public async Task<IHttpActionResult> NextOrder()
        {

            return Ok();
        }

    }
}