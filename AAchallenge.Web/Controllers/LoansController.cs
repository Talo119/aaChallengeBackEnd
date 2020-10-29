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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IEnumerable<LoanViewModel>> List()
        {
            var loan = await _context.Loans.
                Include(l => l.client)
                .OrderByDescending(l => l.idloan)
                .ToListAsync();
            return loan.Select(l => new LoanViewModel
            {
                idloan = l.idloan,
                idclient =l.idclient,
                client = l.client.name,
                capital = l.capital,
                interest_rate = l.interest_rate,
                period = l.period,
                interest_to_pay = l.interest_to_pay,
                amount_to_finance = l.amount_to_finance,
                fee = l.fee,
                created_dt = l.created_dt,
                condicion = l.condicion
            });
        }

        //POST : api/Loans/CreateLoan
        [HttpPost]
        [ActionName("CreateLoan")]
        [Authorize(Roles = "Admin,Credits")]
        public async Task<ActionResult> CreateLoan([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dateTime = DateTime.Now;
            Loan loan = new Loan
            {
                idclient = model.idclient,
                capital = model.capital,
                interest_rate = model.interest_rate,
                period = model.period,
                interest_to_pay = model.interest_to_pay,
                amount_to_finance = model.amount_to_finance,
                fee = model.fee,
                created_dt = dateTime,
                condicion = true
            };
            _context.Loans.Add(loan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        //PUT : api/Loans/Cancel/1
        [HttpPut]
        [ActionName("Cancel")]
        [Authorize(Roles = "Admin,Credits")]
        public async Task<ActionResult> Cancel([FromRoute] int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.idloan == id);
            if (loan == null)
            {
                return NotFound();
            }
            loan.condicion = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok();
        }

        // GET: api/Loans/LoansPerMonth
        
        [HttpGet]
        [ActionName("LoansPerClient")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IEnumerable<ChartViewModel>> LoansPerClient()
        {
            var query = await _context.Loans
                .GroupBy(l => l.client.name)
                .Select(l => new { label = l.Key, value = l.Sum(l => l.amount_to_finance) })
                .OrderByDescending(x => x.label)
                .Take(12)
                .ToListAsync();
            return query.Select(l => new ChartViewModel
            {
                label = l.label.ToString(),
                value = l.value
            });
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.idloan == id);
        }
    }
}
