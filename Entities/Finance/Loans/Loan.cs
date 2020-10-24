using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Entities.Clients;
using Entities.Finance.Payments;

namespace Entities.Finance.Loans
{
    public class Loan
    {
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

        public DateTime created_dt { get; set; }

        public Client client { get; set; }

        public ICollection<Payment> payments { get; set; }

    }
}
