using System;
using System.Collections.Generic;
using System.Text;
using LinqToDB.Mapping;

namespace Shop_Bridge.Domain
{
    [Table(Name = "tblInventoryItem")]
    public class InventoryItemDomain
    {
        [Column, PrimaryKey, Identity]
        public int I_Id { get; set; }
        [Column]
        public string I_Name { get; set; }
        [Column]
        public string I_Description { get; set; }
        [Column]
        public double I_TotalPrice { get; set; }
        [Column]
        public double I_DiscountedPrice { get; set; }
        [Column]
        public int I_PerPackCount { get; set; }
        [Column]
        public int I_ItemCount { get; set; }
        [Column]
        public DateTime I_ExpiryDate { get; set; }
        [Column]
        public bool I_IsActive { get; set; }
        [Column]
        public DateTime I_CreatedOn { get; set; }
        [Column]
        public string I_CreatedBy { get; set; }
        [Column]
        public DateTime I_ModifiedOn { get; set; }
        [Column]
        public string I_ModifiedBy { get; set; }
    }
}
