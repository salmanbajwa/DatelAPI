using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hub.Models;


namespace DatelAPI.Areas.PurchaseOrders.Repositories
{
    public interface IPurchaseOrdersRepository
    {
        Task<SageOrderResponse> CreatePurchaseOrder(PurchaseOrder po);
    }
}
