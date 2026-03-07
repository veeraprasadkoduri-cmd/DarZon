using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SolarPrime.DAL


{
    public class User
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
        [Display(Name = "Mobile Number")]
        [StringLength(10, ErrorMessage = "Mobile No. cannot be longer than 10 characters")]
        public string Mobile { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        
    }
}