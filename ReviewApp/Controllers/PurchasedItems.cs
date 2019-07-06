using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReviewApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReviewApp.Controllers
{
    public class PurchasedItem : Controller
    {
        private readonly ReviewsContext _context;
        private readonly IConfiguration _configuration;

        private string connectionString;
        public PurchasedItem(IConfiguration configuration, ReviewsContext context)
        {
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString("DatabaseConnection");
            _context = context;


        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult MyItems()
        {
            string id = HttpContext.Session.GetString("Id");
            string role = HttpContext.Session.GetString("role");
            if (id == null || role == "buyer" || role =="seller")
            {
                return RedirectToAction("login", "login");
            }
            string Userid = HttpContext.Session.GetString("Id");

            ReviewsContext db = new ReviewsContext();

            List<SellersItems> list = new List<SellersItems>();
            List<byte[]> listImageRefund = new List<byte[]>();
            var items = db.PurchasedItems.ToList().Where(x => x.BuyerId.ToString() == Userid).Select(a => a).ToList();
  

            foreach (var item in items)
            {

                var currentItem = db.SellersItems.ToList().Where(x => x.Id == item.ItemId).Select(x => x).ToList();

                SellersItems itemCurrent = new SellersItems();
                {
                itemCurrent.Categories = item.Id.ToString();
                itemCurrent.CreatedAt = item.DateOfPurchased;
                itemCurrent.Description = item.OrderNumber;
                itemCurrent.Id = currentItem[0].Id;
                itemCurrent.Image = currentItem[0].Image;
                itemCurrent.ItemName = currentItem[0].ItemName;
                itemCurrent.KeyWords = item.Status;
                itemCurrent.Ppfee = currentItem[0].Ppfee;
                itemCurrent.Price = currentItem[0].Price;
                itemCurrent.RefundDaysTime = currentItem[0].RefundDaysTime;
                itemCurrent.ReviesNeeded = currentItem[0].ReviesNeeded;
                itemCurrent.SellerEmail = currentItem[0].SellerEmail;
                itemCurrent.SellerId = currentItem[0].SellerId;
                itemCurrent.StoreName = currentItem[0].StoreName;
            };

            list.Add(itemCurrent);


                if (item.Status == "completed")
                {

                    listImageRefund.Add(item.RefundImage);
                }
                else
                {
                    var oneItem = new byte[] { 0x20 };
                    listImageRefund.Add(oneItem);

                }        

            }

            listImageRefund.Reverse();
            list.Reverse();
            ViewBag.listImageRefund = listImageRefund;
            ViewBag.itemsList = list;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MyItemsPost(string Id, List<IFormFile> files)
        {
            PurchasedItems currentPurchase = new PurchasedItems();

            int currentID = Int32.Parse(Id);
            
            var Currentitem = _context.PurchasedItems.Where(x => x.Id == currentID).Select(x => x).ToList();
            byte[] image = new byte[12];
            if (ModelState.IsValid)
            {
                
                foreach (var item in files)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                             image = stream.ToArray();
                        }
                    }
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("UpdatePurchaseStatus", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@Id", Int32.Parse(Id)));

                    cmd.Parameters.Add(new SqlParameter("@Image", image));


                    // execute the command
                    cmd.ExecuteNonQuery();

                }
                SellersItems itemLive = _context.SellersItems.ToList().Where(x => x.Id == Currentitem[0].ItemId).FirstOrDefault();
                MailMessage mail = new MailMessage("reviewtrades1945@gmail.com", itemLive.SellerEmail);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("reviewtrades1945@gmail.com", "Password1234!");
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                //mail.To.Add("add more peoples...");
                mail.Subject = "Your sold item : " + itemLive.ItemName + " --> review is live!";
                mail.IsBodyHtml = true;

                mail.Body = "<h3>Hi " + itemLive.SellerEmail + "</h3><br>" +
                    "<br><p><b>Your sold item : :</b> " + itemLive.ItemName + " review is live! </p>" +
                    "<br><p>Please login to our website and arange refund Asap</p>" +
                    "<p>Thank you for using our service.<p>" +
                    "<p><b>Reviews Trade Team.</b><p>";
                client.Send(mail);

            }
            return RedirectToAction("Index", "Home");
        }
      
    }
}
