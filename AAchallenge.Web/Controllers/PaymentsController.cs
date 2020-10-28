using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities.Finance.Payments;
using AAchallenge.Web.Models.Finance.Payments;
using Microsoft.AspNetCore.Authorization;

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;

        public PaymentsController(DbContextAAchallenge context)
        {
            _context = context;
        }

        //GET : api/Payments/List
        [HttpGet]
        [ActionName("List")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IEnumerable<PaymentViewModel>> List()
        {
            var payment = await _context.Payments.
                Include(p => p.loan)
                .ToListAsync();

            return payment.Select(p => new PaymentViewModel
            {
                idpayment =  p.idpayment,
                idloan = p.idloan,
                amount = p.amount,
                created_dt = p.created_dt,
                condicion = p.condicion                
            });
        }


        //POST : api/Payments/Create
        [HttpPost]
        [ActionName("Create")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<ActionResult> Create([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dateTime = DateTime.Now;
            Payment payment = new Payment
            {
                idloan = model.idloan,
                amount = model.amount,
                created_dt = dateTime,
                condicion = true
            };
            _context.Payments.Add(payment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        //PUT : api/Payments/Cancel/1
        [HttpPut]
        [ActionName("Cancel")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<ActionResult> Cancel([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.idpayment == id);
            if(payment == null)
            {
                return NotFound();
            }
            payment.condicion = false;
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

        //GET : api/Payments/ListDetail/1
        [HttpGet]
        [ActionName("ListDetail")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IEnumerable<PaymentViewModel>> ListDetail([FromRoute] int id)
        {
            var payment = await _context.Payments.
                Include(p => p.loan)
                .Where(d => d.loan.idloan == id)
                .ToListAsync();

            return payment.Select(p => new PaymentViewModel
            {
                idpayment = p.idpayment,
                idloan = p.idloan,
                amount = p.amount,
                created_dt = p.created_dt,
                condicion = p.condicion
            });
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.idpayment == id);
        }
    }
}
