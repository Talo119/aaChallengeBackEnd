using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AAchallenge.Web.Models.Finance.Payments
{
    public class CreateViewModel
    {
       
        public int idpayment { get; set; }
        [Required]
        public int idloan { get; set; }
        [Required]
        public decimal amount { get; set; }
    }
}
