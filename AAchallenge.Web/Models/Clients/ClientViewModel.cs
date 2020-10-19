using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAchallenge.Web.Models.Clients
{
    public class ClientViewModel
    {
        public int idclient { get; set; }        
        public string name { get; set; }        
        public string address { get; set; }        
        public string phone_number { get; set; }        
        public string email { get; set; }        
        public decimal credit_limit { get; set; }
    }
}
