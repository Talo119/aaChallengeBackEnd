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
                created_dt = p.created_dt
            });
        }


        //POST : api/Payments/Create
        [HttpPost]
        [ActionName("Create")]
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
                created_dt = dateTime
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


        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.idpayment == id);
        }
    }
}
