using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities.Users
{
    public class User
    {
        public int iduser { get; set; }
        [Required]
        public int idrole { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be no more than 100 characters and no less than 3 characters.")]
        public string nombre { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public byte[] password_hash { get; set; }
        [Required]
        public byte[] password_salt { get; set; }
        public bool condicion { get; set; }

        public Role role { get; set; }
    }
}
