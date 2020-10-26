using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities.Users
{
    public class Role
    {
        public int idrole { get; set; }
        [Required]
        [StringLength(30,MinimumLength =3,ErrorMessage = "The name must be no more than 30 characters and no less than 3 characters.")]
        public string name { get; set; }
        [StringLength(100, ErrorMessage = "The name must be no more than 100 characters.")]
        public string description { get; set; }
        public bool condicion { get; set; }

        public ICollection<User> users { get; set; }

    }
}
