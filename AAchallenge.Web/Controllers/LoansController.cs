﻿using System;
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
                created_dt = l.created_dt
            });
        }

        //POST : api/Loans/CreateLoan
        [HttpPost]
        [ActionName("CreateLoan")]
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
                created_dt = dateTime
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

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.idloan == id);
        }
    }
}
