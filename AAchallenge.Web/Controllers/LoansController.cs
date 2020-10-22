using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities.Finance.Loans;
using AAchallenge.Web.Models.Finance.Loans;

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;

        public LoansController(DbContextAAchallenge context)
        {
            _context = context;
        }

        //GET : api/Loans/List
        [HttpGet]
        [ActionName("List")]
        public async Task<IEnumerable<LoanViewModel>> List()
        {
            var loan = await _context.Loans.Include(l => l.client).ToListAsync();
            return loan.Select(a => new LoanViewModel
            {
                idloan = a.idloan,
                idclient = a.idclient,
                client = a.client.name,
                capital = a.capital,
                interest_rate = a.interest_rate,
                period = a.period,
                interest_to_pay = a.interest_to_pay,
                amount_to_finance = a.amount_to_finance,
                fee = a.fee,
                created_dt = a.created_dt
            });
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.idloan == id);
        }
    }
}
