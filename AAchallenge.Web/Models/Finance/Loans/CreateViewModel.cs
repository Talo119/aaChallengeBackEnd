using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AAchallenge.Web.Models.Finance.Loans
{
    public class CreateViewModel
    {
        [Required]
        public int idloan { get; set; }
        [Required]
        public int idclient { get; set; }
        [Required]
        public decimal capital { get; set; }
        [Required]
        public decimal interest_rate { get; set; }
        [Required]
        public int period { get; set; }
        [Required]
        public decimal interest_to_pay { get; set; }
        [Required]
        public decimal amount_to_finance { get; set; }
        [Required]
        public decimal fee { get; set; }
    }
}
