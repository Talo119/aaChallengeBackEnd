using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Entities.Finance.Loans;

namespace Entities.Clients
{
    public class Client
    {
        public int idclient { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Name must not have more than 50 characters, nor less than 3 characters.")]
        public string name { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 30, ErrorMessage = "The Address must not have more than 250 characters, nor less than 30 characters.")]
        public string address { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The Phone number must not have more than 20 characters, nor less than 8 characters.")]
        public string phone_number { get; set; }
        [StringLength(50, MinimumLength = 10, ErrorMessage = "The Email must not have more than 50 characters, nor less than 10 characters.")]
        public string email { get; set; }
        [Required]
        public decimal credit_limit { get; set; }

        public ICollection<Loan> loans {get;set;}


    }
}
