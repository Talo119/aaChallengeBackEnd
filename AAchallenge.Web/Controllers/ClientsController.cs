using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities.Clients;
using AAchallenge.Web.Models.Clients;
using Microsoft.AspNetCore.Authorization;

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;

        public ClientsController(DbContextAAchallenge context)
        {
            _context = context;
        }

        // GET: api/Clients/List
        [HttpGet]        
        [ActionName("List")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IEnumerable<ClientViewModel>> List()
        {
            var client = await _context.Clients.ToListAsync();
            return client.Select(c => new ClientViewModel
            {
                idclient = c.idclient,
                name = c.name,
                address = c.address,
                phone_number= c.phone_number,
                email = c.email,
                credit_limit = c.credit_limit

            });
        }

        // GET: api/Clients/SelectClients
        [HttpGet]
        [ActionName("SelectClients")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IEnumerable<SelectViewModel>> SelectClients()
        {
            var client = await _context.Clients.ToListAsync();
            return client.Select(c => new SelectViewModel
            {
                idclient = c.idclient,
                name = c.name
            });
        }

        // GET: api/Clients/ShowClient/1
        [HttpGet]
        [ActionName("ShowClient")]
        [Authorize(Roles = "Admin,Credits,Charges")]
        public async Task<IActionResult> ShowClient([FromRoute] int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if(client == null)
            {
                return NotFound();
            }
            return Ok(new ClientViewModel
            {
                idclient = client.idclient,
                name = client.name,
                address = client.address,
                phone_number = client.phone_number,
                email = client.email,
                credit_limit = client.credit_limit
            });
        }

        // PUT: api/Clients/ActualizeClient
        [HttpPut]
        [ActionName("ActualizeClient")]
        [Authorize(Roles = "Admin,Credits")]
        public async Task<IActionResult> ActualizeClient([FromBody] ActualizeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idclient < 0)
            {
                return BadRequest();
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.idclient == model.idclient);

            if(client == null)
            {
                return NotFound();
            }

            client.name = model.name;
            client.address = model.address;
            client.phone_number = model.phone_number;
            client.email = model.email;
            client.credit_limit = model.credit_limit;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();

        }

        //POST: api/Clients/CreateClient
        [HttpPost]
        [ActionName("CreateClient")]
        [Authorize(Roles = "Admin,Credits")]
        public async Task<IActionResult> CreateClient([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = new Client
            {
                name = model.name,
                address = model.address,
                phone_number = model.phone_number,
                email = model.email,
                credit_limit = model.credit_limit
            };

            _context.Clients.Add(client);

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

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.idclient == id);
        }
    }
}
