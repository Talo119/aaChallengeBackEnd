using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAchallenge.Web.Models.Users.User
{
    public class UserViewModel
    {
        public int iduser { get; set; }        
        public int idrole { get; set; }  
        public string role { get; set; }
        public string nombre { get; set; }        
        public string email { get; set; }        
        public byte[] password_hash { get; set; }                
        public bool condicion { get; set; }

        
    }
}
