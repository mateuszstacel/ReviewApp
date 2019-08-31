using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReviewApp.Models;

namespace ReviewApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString("DatabaseConnection");

        }

        [HttpPost]
        public IActionResult Register(string login, string password, string email)
        {

            ReviewsContext db = new ReviewsContext();
            List<UsersAccount> CheckIfAccountAlreadyTaken = db.UsersAccount.ToList().Where(a => a.Login == login && a.Password == password)
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
                    cmd.Parameters.Add(new SqlParameter("@Role", "buyer"));


                    cmd.ExecuteNonQuery();

                }

            }

            ModelState.AddModelError("error", "Student Name already exists.");

            return View("Login"); ;
        }


    }

}