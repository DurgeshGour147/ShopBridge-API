using iText.Html2pdf;
using Shop_Bridge.Common;
using Shop_Bridge.Domain;
using Shop_Bridge.DTO;
using Shop_Bridge.ProviderInterface;
using Shop_Bridge.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shop_Bridge.Provider
{
    public class ShopBridgeProvider : IShopBridgeProvider
    {

        private readonly IDBRepository _DBProvider;

        public ShopBridgeProvider(IDBRepository dbRepository)
        {
            _DBProvider = dbRepository;
        }

        public async Task<BaseResponseDTO> ActionInventoryItems(List<InventoryRequestDTO> request)
        {
            if (!request.HasRecords())
                return new BaseResponseDTO { ErrorMessage = ErrorConstant.InValidRequest };

            List<InventoryItemDomain> requestDomain = new List<InventoryItemDomain>();
            request.ForEach(x =>
            {
                requestDomain.Add(new InventoryItemDomain()
                {
                    I_Id = x.Id,
                    I_Description = x.Description,
                    I_DiscountedPrice = x.DiscountedPrice,
                    I_ExpiryDate = x.ExpiryDate,
                    I_IsActive = true,
                    I_ItemCount = x.ItemCount,
                    I_Name = x.Name,
                    I_PerPackCount = x.PerPackCount,
                    I_TotalPrice = x.TotalPrice

                });
            });
            bool result = await _DBProvider.ActionOnIntentoryItem(requestDomain);
            return new BaseResponseDTO { IsSuccess = result };
        }

        public async Task<BaseResponseDTO> DeleteInventoryItems(List<int> id)
        {
            if (!id.HasRecords())
                return new BaseResponseDTO { ErrorMessage = ErrorConstant.InValidRequest };
            bool result = await _DBProvider.InActiveInventoryItem(id);
            return new BaseResponseDTO() { IsSuccess = result };
        }

        public async Task<List<InventoryResponseDTO>> GetInventoryItems(InventoryItemRequest request)
        {
            if (request.IsNull())
                return null;

            Expression<Func<InventoryItemDomain, bool>> expression = null;
            if (!string.IsNullOrEmpty(request.Name) || request.Id > 0)
            {
                expression = x => x.I_IsActive;
                if (!string.IsNullOrEmpty(request.Name))
                    expression = expression.ExpressionAnd(x => x.I_Name == request.Name);
                if (request.Id > 0)
                    expression = expression.ExpressionAnd(x => x.I_Id == request.Id);
            }

            request.PageNumber = request.PageNumber == default ? 1 : request.PageNumber;
            request.PageSize = request.PageSize == default ? 10 : request.PageSize;
            List<InventoryItemDomain> resp = await _DBProvider.GetIntentoryItem(expression, request.PageNumber, request.PageSize);
            if (!resp.HasRecords())
                return null;

            List<InventoryResponseDTO> result = new List<InventoryResponseDTO>();
            resp.ForEach(x =>
            {
                result.Add(new InventoryResponseDTO()
                {
                    CreatedBy = x.I_CreatedBy,
                    CreatedOn = x.I_CreatedOn,
                    Description = x.I_Description,
                    DiscountedPrice = x.I_DiscountedPrice,
                    ExpiryDate = x.I_ExpiryDate,
                    Id = x.I_Id,
                    IsActive = x.I_IsActive,
                    ItemCount = x.I_ItemCount,
                    ModifiedBy = x.I_ModifiedBy,
                    ModifiedOn = x.I_ModifiedOn,
                    Name = x.I_Name,
                    PerPackCount = x.I_PerPackCount,
                    TotalPrice = x.I_TotalPrice
                });
            });
            return result;
        }
    }
}
