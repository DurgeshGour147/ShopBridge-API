using Shop_Bridge.DTO;
using Shop_Bridge.Filters;
using Shop_Bridge.ProviderInterface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shop_Bridge.Common;
using System.Collections.Generic;

namespace Shop_Bridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopBridgeController : ControllerBase
    {
        private readonly IShopBridgeProvider _ShopBridgeProvider;
        public ShopBridgeController(IShopBridgeProvider ShopBridgeProvider)
        {
            _ShopBridgeProvider = ShopBridgeProvider;
        }

        [AuthorizationFilter(500)]
        [HttpPost("ActionInventoryItems")]
        public async Task<BaseResponseDTO> AddInventoryItems(InventoryReqDTO request)
        {
            if (request.IsNull() || string.IsNullOrEmpty(request.AccessToken))
                return new BaseResponseDTO { ErrorMessage = ErrorConstant.InValidRequest };

            return await _ShopBridgeProvider.ActionInventoryItems(request.Items);
        }

        [HttpPost("FetchInventoryItems")]
        public async Task<List<InventoryResponseDTO>> FetchInventoryItems(InventoryItemRequest request)
        {
            return await _ShopBridgeProvider.GetInventoryItems(request);
        }


        [AuthorizationFilter(500)]
        [HttpPost("DeleteInventoryItems")]
        public async Task<BaseResponseDTO> DeleteInventoryItem(DeleteInventoryItemDTO request)
        {
            if (request.IsNull() || string.IsNullOrEmpty(request.AccessToken))
                return new BaseResponseDTO { ErrorMessage = ErrorConstant.InValidRequest };

            return await _ShopBridgeProvider.DeleteInventoryItems(request.Ids);
        }
    }
}
