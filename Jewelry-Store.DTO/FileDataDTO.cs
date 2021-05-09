using System;
using System.Collections.Generic;
using System.Text;

namespace Jewelry_Store.DTO
{
    public class FileDataRequestDTO : BaseRequestDTO
    {
        public string GoldPrice { get; set; }
        public string Weight { get; set; }
        public string DiscountInPercentage { get; set; }
        public string TotalPrice { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
                return false;

            if (string.IsNullOrEmpty(this.GoldPrice))
                return false;
            if (string.IsNullOrEmpty(this.Weight))
                return false;
            if (string.IsNullOrEmpty(this.TotalPrice))
                return false;

            return true;
        }
    }
}
