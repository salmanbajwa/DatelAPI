using DatelAPI.Areas.SalesOrders.Repositories;
using Hub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace DatelAPI.Areas.SalesOrders.Controllers
{
    [RoutePrefix("sage/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrdersRepository _repository;

        public OrdersController(IOrdersRepository repository)
        {
            _repository = repository;
        }


        [Route ("Save")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateOrder(SalesOrder so)
        {
            SageOrderResponse result = await _repository.CreateSalesOrder(so);
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
