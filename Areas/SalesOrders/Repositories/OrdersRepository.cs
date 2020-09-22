using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using FusionSDK;
using DatelAPI.Areas.Config;
using DatelAPI.Areas.Data;
using DatelAPI.Areas.Logger;
using Hub.Models;

namespace DatelAPI.Areas.SalesOrders.Repositories
{
    public class OrdersRepository: IOrdersRepository
    {
        private readonly IConfigProvider _config;
        private readonly ISageData _data;
        private readonly ILogger _logger;
        public OrdersRepository(IConfigProvider config, ISageData data, ILogger logger)
        {
            _config = config;
            _data = data;
            _logger = logger;
        }

        public async Task<SageOrderResponse> CreateSalesOrder(Hub.Models.SalesOrder so)
        {
            SageOrderResponse result = new SageOrderResponse();
            try
            {
                Dictionary<string, string> SystemKeys = new Dictionary<string, string>();
                SystemKeys = await _data.GetDBLoginDetails(_config.DatelSystemID);

                if (SystemKeys.Count == 0)
                {
                    _logger.Log("Error: Unable to find System Keys");
                    return result;
                }
                
                FusionSDK.SalesOrder salesAPI = new FusionSDK.SalesOrder();
                salesAPI.setSQL(SystemKeys["dbServer"], SystemKeys["dbName"], SystemKeys["dbLogin"], SystemKeys["dbPassword"]);
                salesAPI.setDeveloperDebuggingMode(_config.SageLogging);
                salesAPI.setSchema(SystemKeys["dbScheme"]);
                salesAPI.setOrderPrefix(SystemKeys["orderPrefix"].Trim().ToUpper());

                salesAPI.setSalesOrderTrackingUser(SystemKeys["dbLogin"].Substring(0, 8));
                salesAPI.setAuditFile(Convert.ToBoolean(SystemKeys["auditMode"]));

                
                salesAPI.setHeaderValueString("customer_order_no", so.CustomerRef.ID);

                salesAPI.setHeaderValueString("lang", so.Delivery.DelAddr.DelCountry.Name);
                salesAPI.setHeaderValueString("transaction_anals1", so.Delivery.DelParty.EndPoint);
                salesAPI.setHeaderValueString("transaction_anals3", so.Delivery.DelParty.Logo);
                if (! String.IsNullOrEmpty(so.additionalData.alpha))
                {
                    salesAPI.setHeaderValueString("alpha", so.additionalData.alpha);
                }
                if (so.Delivery.DelParty.DelContact.Tel != "")
                {
                    salesAPI.setHeaderValueString("shipping_text", so.Delivery.DelParty.DelContact.Tel);
                }

                foreach (var line in so.OrderLines)
                {
                    salesAPI.addGoodsLineWithPrices(line.Item.ItemData.Warehouse, line.Item.ItemData.SKU, Convert.ToDouble(line.Item.Qty), line.Item.ItemData.ListPrice
                        , line.Item.ItemData.NetPrice, line.Item.ItemData.Discount, line.Item.ItemData.DiscountAllowed);

                    //salesAPI.setDetailValueString("packaging", JavaCast('string', arguments.accessories[k].pairid));
                    salesAPI.setDetailValueString("transaction_anals3", so.Delivery.DelParty.Logo);
                }
                foreach (var comment in so.additionalData.Comments)
                {
                    salesAPI.addCommentLine(comment.Type, comment.Text);
                }
                foreach (var service in so.additionalData.ServiceLines)
                {
                    salesAPI.addServiceLineWithPrices(service.Item.ItemData.SKU, Convert.ToDouble(service.Item.Qty), service.Item.ItemData.ListPrice, service.Item.ItemData.NetPrice
                        , service.Item.ItemData.Discount, service.Item.ItemData.DiscountAllowed);
                    salesAPI.setDetailValueString("long_description", service.Item.ItemData.Description);
                }

                salesAPI.addServiceLineWithPrices(so.DelTerms.DelCode, 1, 0, 0, 0, false);

                salesAPI.setHeaderValueString("address1", so.Address.ContactName);
                salesAPI.setHeaderValueString("address2", so.Address.Address1);
                salesAPI.setHeaderValueString("address3", so.Address.Address2);
                salesAPI.setHeaderValueString("address4", so.Address.City);
                salesAPI.setHeaderValueString("address5", so.Address.PostCode);

                salesAPI.setHeaderValueString("delivery_method", so.additionalData.DeliveryMethod);

                salesAPI.setHeaderValueString("hold_indicator", so.additionalData.hold_indicator);

                salesAPI.setHeaderValueString("date_entered", DateTime.Now.ToString("yyyy-MM-dd"));
                salesAPI.setHeaderValueString("date_received", DateTime.Now.ToString("yyyy-MM-dd"));

                string SageOrderRef = "";
                string ErrorDesc = "";

                if (salesAPI.submitOrder(so.additionalData.CustomerID))
                {
                    SageOrderRef = salesAPI.getOrderNumber();
                    result.OrderRef = SageOrderRef;
                    result.OrderPlaced = true;

                    _logger.Log($"Sales Order creation successful: {SageOrderRef}");
                }
                else
                {
                    ErrorDesc = $"ErrorCode: {salesAPI.getLastErrorCode()}. ErrorMessage: {salesAPI.getLastErrorMessage()}. SQLError: {salesAPI.getLastSQLErrorCode()} ";

                    result.ErrorMessage = ErrorDesc;
                    _logger.Log($"Error: Sales Order creation failed: {so.CustomerRef.ID}. {ErrorDesc} ");
                }

                if (SageOrderRef != "")
                {
                    OrderAllocation allocation = new OrderAllocation();

                    allocation.setSQL(SystemKeys["dbServer"], SystemKeys["dbName"], SystemKeys["dbLogin"], SystemKeys["dbPassword"]);
                    allocation.setDeveloperDebuggingMode(_config.SageLogging);
                    allocation.setSchema(SystemKeys["dbScheme"]);
                    allocation.allowSplitToBackOrder(so.additionalData.AllowBackOrder);
                    allocation.setSoftAllocationOnly(Convert.ToBoolean(SystemKeys["softAllocationOnly"]));
                    allocation.setSalesOrderTrackingUser(SystemKeys["dbLogin"].Substring(0, 8));
                    allocation.setAuditFile(Convert.ToBoolean(SystemKeys["auditMode"]));

                    result.StockAllocated = allocation.doAllocation(SageOrderRef);

                    if (!result.StockAllocated)
                    {
                        result.ErrorMessage = allocation.getLastErrorMessage();

                        if (result.ErrorMessage.Contains("Rerun the transaction"))
                        {
                            result.StockAllocated = allocation.doAllocation(SageOrderRef);

                            if (result.StockAllocated)
                            {
                                result.ErrorMessage = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
            return result;
        }
    }
}