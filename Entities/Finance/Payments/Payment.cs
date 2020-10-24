using Entities.Finance.Loans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Finance.Payments
{
    public class Payment
    {
        public int idpayment { get; set; }
        [Required]
        public int idloan { get; set; }
        [Required]
        public decimal amount { get; set; }
        [Required]
        public DateTime created_dt { get; set; }
        public bool condicion { get; set; }

        public Loan loan { get; set; }

    }
}
