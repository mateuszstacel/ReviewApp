using System;
using System.Collections.Generic;

namespace ReviewApp.Models
{
    public partial class PurchasedItems
    {
        public int Id { get; set; }
        public int? BuyerId { get; set; }
        public int? SellerId { get; set; }
        public int? ItemId { get; set; }
        public DateTime? DateOfPurchased { get; set; }
        public byte[] ScreenShoot { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public string Ppemail { get; set; }
        public byte[] ReviewScreenshot { get; set; }
        public byte[] RefundImage { get; set; }
    }
}
