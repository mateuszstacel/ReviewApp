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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReviewApp.Models;

namespace ReviewApp
{
    public class SellersItemsController : Controller
    {
        private readonly ReviewsContext _context;
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public SellersItemsController(IConfiguration configuration, ReviewsContext context)
        {
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString("DatabaseConnection");
            _context = context;
        }


        // GET: SellersItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Id = HttpContext.Session.GetString("Id");
            ViewBag.role = HttpContext.Session.GetString("role");

            if (id == null)
            {
                return NotFound();
            }

            var sellersItems = await _context.SellersItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellersItems == null)
            {
                return NotFound();
            }

            return View(sellersItems);
        }

        // GET: SellersItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SellersItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Image,Price,ItemName,Description,StoreName,KeyWords,ReviesNeeded,RefundDaysTime,Ppfee,Categories")] SellersItems sellersItems, List<IFormFile> files)
        {

            string id = HttpContext.Session.GetString("Id");
            string role = HttpContext.Session.GetString("role");
            if (id == null || role != "seller")
            {
                return RedirectToAction("login", "login");
            }
            if (ModelState.IsValid)
            {

                foreach (var item in files)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            sellersItems.Image = stream.ToArray();
                        }
                    }
                }

                int sellerId = int.Parse(HttpContext.Session.GetString("Id"));
                string email = HttpContext.Session.GetString("Email");
                sellersItems.SellerEmail = email;
                sellersItems.SellerId = sellerId;
                //int maxValue = _context.SellersItems.Max(x => x.Id);
                //sellersItems.Id = maxValue + 1;
                sellersItems.CreatedAt = DateTime.Now;
                _context.Add(sellersItems);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(sellersItems);
        }

        // GET: SellersItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string role = HttpContext.Session.GetString("role");
            ViewBag.role = HttpContext.Session.GetString("role");
            if ( role != "seller")
            {
                return RedirectToAction("login", "login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var sellersItems = await _context.SellersItems.FindAsync(id);
            if (sellersItems == null)
            {
                return NotFound();
            }
            return View(sellersItems);
        }

        // POST: SellersItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Price,ItemName,Description,StoreName,KeyWords,ReviesNeeded,RefundDaysTime,Ppfee,Categories")] SellersItems sellersItems, List<IFormFile> files)
        {
            string role = HttpContext.Session.GetString("role");
            if ( role != "seller")
            {
                return RedirectToAction("login", "login");
            }

            if (id != sellersItems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in files)
                    {
                        if (item.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await item.CopyToAsync(stream);
                                sellersItems.Image = stream.ToArray();
                            }
                        }
                    }
                    sellersItems.CreatedAt = DateTime.Now;
                    sellersItems.SellerId = Int32.Parse(HttpContext.Session.GetString("Id"));
                    sellersItems.SellerEmail = HttpContext.Session.GetString("Email");
                    _context.Update(sellersItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellersItemsExists(sellersItems.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(sellersItems);
        }

        // GET: SellersItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellersItems = await _context.SellersItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellersItems == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: SellersItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sellersItems = await _context.SellersItems.FindAsync(id);
            _context.SellersItems.Remove(sellersItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> PurchaseItemPost(string orderNum, string paypal,int itemId, int sellerId, List<IFormFile> files)
        {
            PurchasedItems currentPurchase = new PurchasedItems();
            currentPurchase.OrderNumber = orderNum;
            currentPurchase.Ppemail = paypal;
            currentPurchase.DateOfPurchased = DateTime.Now;
            currentPurchase.ItemId = itemId;
            currentPurchase.SellerId = sellerId;
            currentPurchase.BuyerId = Int32.Parse(HttpContext.Session.GetString("Id"));
            currentPurchase.Status = "pending";
            if (ModelState.IsValid)
            {

                foreach (var item in files)
                {
                    if (item.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            currentPurchase.ScreenShoot = stream.ToArray();
                        }
                    }
                }
  
                _context.Add(currentPurchase);
                await _context.SaveChangesAsync();

                var itemToUpdate = _context.SellersItems.Where(x => x.Id == itemId).Select(x => x);

                int? HowmanyReviews = 0;

                foreach (var item in itemToUpdate)
                {
                   HowmanyReviews = item.ReviesNeeded - 1;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("ReviewsNeeded", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@number", HowmanyReviews));

                    cmd.Parameters.Add(new SqlParameter("@Id", itemId));


                    // execute the command
                    cmd.ExecuteNonQuery();




                }
                ReviewsContext db = new ReviewsContext();

                UsersAccount seller = db.UsersAccount.ToList().Where(x => x.Id == sellerId).FirstOrDefault();
               SellersItems soldItem = db.SellersItems.ToList().Where(x => x.Id == itemId).FirstOrDefault();


                MailMessage mail = new MailMessage("reviewtrades1945@gmail.com", seller.Email);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("reviewtrades1945@gmail.com", "Password1234!");
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                //mail.To.Add("add more peoples...");
                mail.Subject = "You have Sold " + soldItem.ItemName + "!";
                mail.IsBodyHtml = true;

                mail.Body = "<h3>Hi " + seller.Email + "</h3><br>" +
                    "<br><p><b>You have sold :</b> " + soldItem.ItemName + "</p>" +
                    "<b>Order number : </b>" + orderNum +
                    "<br><p>we will let you know once review will be live.</p>" +
                    "<br><p>Thank you for using our service<p>"+
                    "<p><b>Reviews Trade Team.</b><p>";
                client.Send(mail);


                return RedirectToAction("MyItems", "PurchasedItem");
            }
            return View();
        }


        private bool SellersItemsExists(int id)
        {
            return _context.SellersItems.Any(e => e.Id == id);
        }

        public IActionResult MyItems()
        {
            string id = HttpContext.Session.GetString("Id");
            string role = HttpContext.Session.GetString("role");
            if (id == null || role != "seller")
            {
                return RedirectToAction("login", "login");
            }

            string Id = HttpContext.Session.GetString("Id");
            ViewBag.role = HttpContext.Session.GetString("role");
            ReviewsContext db = new ReviewsContext();

            ViewBag.list = db.SellersItems.ToList().Where(x => x.SellerId.ToString() == Id).Select(x => x).ToList();
            return View();
        }

        public IActionResult SoldItems()
        {
            string idd = HttpContext.Session.GetString("Id");
            string role = HttpContext.Session.GetString("role");
            if (idd == null || role != "seller")
            {
                return RedirectToAction("login", "login");
            }

            ReviewsContext db = new ReviewsContext();

            List<SellersItems> list = new List<SellersItems>();

            string id = HttpContext.Session.GetString("Id");

            var lists = db.PurchasedItems.ToList().Where(x => x.SellerId.ToString() == id).Select(x => x).ToList();

            foreach (var item in lists)
            {
                var currentItem = db.SellersItems.ToList().Where(x => x.Id == item.ItemId).Select(x => x).ToList();
                ViewBag.Paypal = item.Ppemail;
                SellersItems itemCurrent = new SellersItems();
                {
                    itemCurrent.Categories = currentItem[0].Categories;
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


            }
            list.Reverse();
            ViewBag.itemsList = list;
            return View();
        }


        public IActionResult ItemsToRefund()
        {

            string idd = HttpContext.Session.GetString("Id");
            string role = HttpContext.Session.GetString("role");
            if (idd == null || role != "seller")
            {
                return RedirectToAction("login", "login");
            }

            ReviewsContext db = new ReviewsContext();

            List<SellersItems> list = new List<SellersItems>();

            string id = HttpContext.Session.GetString("Id");

            var lists = db.PurchasedItems.ToList().Where(x => x.SellerId.ToString() == id && x.Status == "reviewLive").Select(x => x).ToList();
            List<byte[]> Image2 = new List<byte[]>();
            List<int> listID = new List<int>();
            foreach (var item in lists)
            {
                var currentItem = db.SellersItems.ToList().Where(x => x.Id == item.ItemId).Select(x => x).ToList();
                ViewBag.Paypal = item.Ppemail;
                
                SellersItems itemCurrent = new SellersItems();
                
                {
                    itemCurrent.Categories = currentItem[0].Categories;
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

                };
                list.Add(itemCurrent);
                Image2.Add(item.ReviewScreenshot);
                listID.Add(item.Id);

            }
            ViewBag.ImageReviewLive = Image2;
            ViewBag.itemsList = list;
            ViewBag.listId = listID;
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
                    SqlCommand cmd = new SqlCommand("CompletePurchase", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@Id", Int32.Parse(Id)));

                    cmd.Parameters.Add(new SqlParameter("@Image", image));


                    // execute the command
                    cmd.ExecuteNonQuery();

                }

                SellersItems refundedItem = _context.SellersItems.Where(x => x.Id == Currentitem[0].ItemId).FirstOrDefault();
                UsersAccount buyerId = _context.UsersAccount.Where(x => x.Id == Currentitem[0].BuyerId).FirstOrDefault();


                MailMessage mail = new MailMessage("reviewtrades1945@gmail.com", buyerId.Email);
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("reviewtrades1945@gmail.com", "Password1234!");
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                mail.Subject = "You have refund for<b> " + refundedItem.ItemName + "!</b>";
                mail.IsBodyHtml = true;

                mail.Body = "<h3>Hi " + buyerId.Email + "</h3><br>" +
                    "<br><p><b>You have refund for :</b> " + refundedItem.ItemName + "</p>" +
                    "<br><p>Please login to our service to see refund screenshot.</p>" +
                    "<br><p>Thank you for using our service.<p>" +
                    "<p><b>Reviews Trade Team.</b><p>";
                client.Send(mail);


            }
            return RedirectToAction("Index", "Home");
        }
    }
}
