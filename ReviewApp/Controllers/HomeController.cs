using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReviewsContext _context;

        public HomeController(ReviewsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string id = HttpContext.Session.GetString("Id");

            var purchasedList = _context.PurchasedItems.ToList(). Where(x => x.BuyerId.ToString() == id).Select(x => x).ToList();

            List<string> itemsID = new List<string>();

            foreach (var item in purchasedList)
            {
                itemsID.Add(item.ItemId.ToString());
            }

            ViewBag.itemsID = itemsID;

            if (id == null)
            {
               return RedirectToAction("login", "login");
            }
            string role = HttpContext.Session.GetString("role");

            ViewBag.role = role;

           

            return View(await _context.SellersItems.ToListAsync());
        }

       

    }
}
