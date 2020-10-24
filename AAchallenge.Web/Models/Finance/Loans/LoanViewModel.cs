using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AAchallenge.Web.Models.Finance.Loans
{
    public class LoanViewModel
    {
        public int idloan { get; set; }        
        public int idclient { get; set; }
        public string client { get; set; }
        public decimal capital { get; set; }        
        public decimal interest_rate { get; set; }        
        public int period { get; set; }        
        public decimal interest_to_pay { get; set; }        
        public decimal amount_to_finance { get; set; }        
        public decimal fee { get; set; }
        public DateTime created_dt { get; set; }
        public bool condicion { get; set; }
    }
}
