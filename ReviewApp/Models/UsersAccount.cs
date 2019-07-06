using System;
using System.Collections.Generic;

namespace ReviewApp.Models
{
    public partial class UsersAccount
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Ip { get; set; }
        public string ComputerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }
    }
}
