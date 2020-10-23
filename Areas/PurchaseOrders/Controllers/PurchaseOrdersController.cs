using Hub.Models;
using DatelAPI.Areas.PurchaseOrders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DatelAPI.Areas.Logger;

namespace DatelAPI.Areas.PurchaseOrders.Controllers
{
    [RoutePrefix("sage/purchaseorders")]
    public class PurchaseOrdersController : ApiController
    {
        private readonly IPurchaseOrdersRepository _repository;
        private readonly ILogger _logger;
        public PurchaseOrdersController(IPurchaseOrdersRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
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
            _logger.Log("Inside Test");
            return Ok(100);
        }

    }
}