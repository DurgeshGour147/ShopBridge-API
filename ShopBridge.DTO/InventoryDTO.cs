using System;
using System.Collections.Generic;
using System.Text;

namespace Shop_Bridge.DTO
{
    
    public class DeleteInventoryItemDTO : BaseRequestDTO
    {
        public List<int> Ids { get; set; }
    }

    public class InventoryReqDTO : BaseRequestDTO
    {
        public List<InventoryRequestDTO> Items { get; set; }
    }

    public class InventoryRequestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double TotalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public int PerPackCount { get; set; }
        public int ItemCount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class InventoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double TotalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public int PerPackCount { get; set; }
        public int ItemCount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class InventoryItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
