using System;
using System.Collections.Generic;

namespace ReviewApp.Models
{
    public partial class SellersItems
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Price { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string StoreName { get; set; }
        public string KeyWords { get; set; }
        public int? ReviesNeeded { get; set; }
        public int? RefundDaysTime { get; set; }
        public bool? Ppfee { get; set; }
        public string Categories { get; set; }
        public int? SellerId { get; set; }
        public string SellerEmail { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
