using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Server.Models
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email must be not empty.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password must be not empty.")]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
