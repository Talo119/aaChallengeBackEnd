using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AAchallenge.Web.Models.Finance.Payments
{
    public class PaymentViewModel
    {
        public int idpayment { get; set; }        
        public int idloan { get; set; }        
        public decimal amount { get; set; }
        public bool condicion { get; set; }
        public DateTime created_dt { get; set; }

        
    }
}
