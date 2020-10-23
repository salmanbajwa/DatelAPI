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


namespace DatelAPI.Areas.PurchaseOrders.Repositories
{
    public class PurchaseOrderRepository: IPurchaseOrdersRepository
    {
        private readonly IConfigProvider _config;
        private readonly ISageData _data;
        private readonly ILogger _logger;

        public PurchaseOrderRepository(IConfigProvider config, ISageData data, ILogger logger)
        {
            _config = config;
            _data = data;
            _logger = logger;
        }

        public async Task<SageOrderResponse> CreatePurchaseOrder(Hub.Models.PurchaseOrder po)
        {
            SageOrderResponse result = new SageOrderResponse();
            result.ErrorCode = "0";
            
            result.OrderPlaced = false;
            try
            {
                _logger.Log("Inside PurchaseOrder");

                result.ErrorCode = "-1";

                Dictionary<string, string> SystemKeys = new Dictionary<string, string>();
                SystemKeys = await _data.GetDBLoginDetails(_config.DatelSystemID);

                result.ErrorCode = "-2";
                if (SystemKeys.Count == 0)
                {
                    _logger.Error("Error: Unable to find System Keys");
                    return result;
                }

                result.ErrorCode = "-3";
                FusionSDK.PurchaseOrder poAPI = new FusionSDK.PurchaseOrder();
                result.ErrorCode = "-4";
                poAPI.setSQL(SystemKeys["dbServer"], SystemKeys["dbName"], SystemKeys["dbLogin"], SystemKeys["dbPassword"]);
                poAPI.setDeveloperDebuggingMode(_config.SageLogging);
                poAPI.setSchema(SystemKeys["dbScheme"]);
                

                poAPI.setHeaderValueString("supplier_ref", po.CustomerRef.ID);

                poAPI.setHeaderValueString("status", "P");
                poAPI.setHeaderValueString("supplier", po.additionalData.Supplier);
                poAPI.setHeaderValueString("address1", po.Seller.Password);
                poAPI.setHeaderValueString("pl_company", SystemKeys["dbName"]);
                poAPI.setHeaderValueString("exchange_rate", "0");
                poAPI.setHeaderValueString("vat_type", "V");

                foreach (var line in po.OrderLines)
                {
                    poAPI.addGoodsLineValue(line.Item.ItemData.Warehouse, line.Item.ItemData.SKU, Convert.ToDouble(line.Item.Qty), line.Item.ItemData.ListPrice);
                    if (!String.IsNullOrEmpty(line.Item.Delivery.DeliveryDateValue))
                    {
                        poAPI.setDetailValueString("date_required", line.Item.Delivery.DeliveryDateValue);
                    }
                    /*Serials */
                    //for (m = 1; m lte ArrayLen(arguments.accessories[k].serial); m = m + 1)
						//{
                        //po.addCommentLine(trim(arguments.accessories[k].serial[m].type), trim(arguments.accessories[k].serial[m].value));
                    //}

                }

                foreach (var comment in po.additionalData.Comments)
                {
                    poAPI.addCommentLine(comment.Type, comment.Text);
                }

                poAPI.setHeaderValueString("delivery_address1", po.Address.ContactName);
                poAPI.setHeaderValueString("delivery_address2", po.Address.Address1);
                poAPI.setHeaderValueString("delivery_address3", po.Address.Address2);
                poAPI.setHeaderValueString("delivery_address4", po.Address.City);
                poAPI.setHeaderValueString("delivery_address5", po.Address.PostCode);

                string SageOrderRef = "";
                string ErrorDesc = "";

                result.ErrorCode = "1";

                if (poAPI.submitOrder(po.additionalData.Supplier))
                {
                    result.ErrorCode = "2";
                    SageOrderRef = poAPI.getOrderNumber();
                    result.ErrorCode = "3";
                    result.OrderRef = SageOrderRef;
                    result.OrderPlaced = true;
                    result.ErrorCode = "4";
                    _logger.Log($"Purchase Order creation successful: {SageOrderRef}");
                }
                else
                {
                    ErrorDesc = $"ErrorCode: {poAPI.getLastErrorCode()}. ErrorMessage: {poAPI.getLastErrorMessage()}. SQLError: {poAPI.getLastSQLErrorCode()} ";

                    result.ErrorMessage = ErrorDesc;
                    _logger.Log($"Error: Purchase Order creation failed: {po.CustomerRef.ID}. {ErrorDesc} ");
                }

            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                result.ErrorMessage = ex.Message;
                throw ex;
            }
            return result;
        }
    }
}