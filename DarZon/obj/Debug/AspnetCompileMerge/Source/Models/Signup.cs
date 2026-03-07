using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DarZon.Models
{
    public  class Signup
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required.")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string Roles { get; set; }


    }
}

