using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    public class LoginController : Controller
    {

        private readonly IConfiguration _configuration;
        private string connectionString;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString("DatabaseConnection");

        }

        [HttpGet]
        public IActionResult Login()
        {


            return View();
        }




[HttpPost]
public IActionResult Login(string login, string password)
{
    ReviewsContext db = new ReviewsContext();
    List<UsersAccount> CheckUserCredentials = db.UsersAccount.ToList().Where(a => a.Login == login && a.Password == password)
                                                .Select(a => a).ToList();
    if (CheckUserCredentials.Count() != 0)
    {
        HttpContext.Session.SetString("Login", login);
        HttpContext.Session.SetString("Email", CheckUserCredentials[0].Email);
        HttpContext.Session.SetString("Id", Convert.ToString(CheckUserCredentials[0].Id));
        HttpContext.Session.SetString("firstName", CheckUserCredentials[0].FirstName);
        HttpContext.Session.SetString("lastName", CheckUserCredentials[0].LastName);
        HttpContext.Session.SetString("role", CheckUserCredentials[0].Role);


        //using (SqlConnection conn = new SqlConnection(connectionString))
        //{

        //    conn.Open();
        //    DateTime loginTime = DateTime.Now;
        //    // 1.  create a command object identifying the stored procedure
        //    SqlCommand cmd = new SqlCommand("Update UsersAccount Set LastLogin VALUES = " + "'" + loginTime + "'" + " Where ID = " + CheckUserCredentials[0].Id + "", conn);
        //    cmd.Parameters.AddWithValue("@value", loginTime);
        //    cmd.Parameters.AddWithValue("@ID", CheckUserCredentials[0].Id);

        //    cmd.ExecuteNonQuery();

        //}
        return RedirectToAction("Index", "Home");

    }
    ViewBag.message = "Login and Password not match, please try again";
    ViewBag.classinfo = "alert alert-danger";
    return View();
}

[HttpPost]
public IActionResult Register(string firstName, string lastName, string login, string password, string email)
{

    ReviewsContext db = new ReviewsContext();
    List<UsersAccount> CheckIfAccountAlreadyTaken = db.UsersAccount.ToList().Where(a => a.Login == login)
                                                                                                            .Select(a => a).ToList();

    if (CheckIfAccountAlreadyTaken.Count() == 0)
    {
        string computerName = Environment.MachineName;
        string ip = "";

        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var Ip in host.AddressList)
        {
            if (Ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ip = Ip.ToString();
            }
        };

        using (SqlConnection conn = new SqlConnection(connectionString))
        {

            conn.Open();

            // 1.  create a command object identifying the stored procedure
            SqlCommand cmd = new SqlCommand("NewUser", conn);

            // 2. set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // 3. add parameter to command, which will be passed to the stored procedure
            cmd.Parameters.Add(new SqlParameter("@login", login));
            cmd.Parameters.Add(new SqlParameter("@password", password));
            cmd.Parameters.Add(new SqlParameter("@email", email));
            cmd.Parameters.Add(new SqlParameter("@IP", ip));
            cmd.Parameters.Add(new SqlParameter("@ComputerName", computerName));
            cmd.Parameters.Add(new SqlParameter("@FirstName", firstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", lastName));
            cmd.Parameters.Add(new SqlParameter("@Role", "buyer"));


            cmd.ExecuteNonQuery();

        }
        ViewBag.message = "Registration Complete successfully please login.";
        ViewBag.classinfo = "alert alert-success";
    }
    else
    {
        ViewBag.message = "User with this login already exist please try again.";
        ViewBag.classinfo = "alert alert-danger";
    }


    return View("Login");
}
    
}}
