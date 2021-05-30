using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop_Bridge.DTO;

namespace Shop_Bridge.ProviderInterface
{
    public interface IShopBridgeProvider
    {
        Task<BaseResponseDTO> ActionInventoryItems(List<InventoryRequestDTO> request);
        Task<BaseResponseDTO> DeleteInventoryItems(List<int> id);
        Task<List<InventoryResponseDTO>> GetInventoryItems(InventoryItemRequest request);
    }
}
