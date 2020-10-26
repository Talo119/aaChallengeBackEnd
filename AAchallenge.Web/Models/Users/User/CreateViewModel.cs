using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AAchallenge.Web.Models.Users.User
{
    public class CreateViewModel
    {        
        [Required]
        public int idrole { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be no more than 100 characters and no less than 3 characters.")]
        public string nombre { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        
    }
}
