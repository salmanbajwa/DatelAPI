using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Models;

namespace DatelAPI.Areas.SalesOrders.Repositories
{
    public interface IOrdersRepository
    {
        Task<SageOrderResponse> CreateSalesOrder(SalesOrder so);

        Task<SageOrderResponse> AllocateStock(string OrderRef);
    }
}
