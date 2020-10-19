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

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;

        public ClientsController(DbContextAAchallenge context)
        {
            _context = context;
        }

        // GET: api/Clients/List
        [HttpGet("[action]")]
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
            

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.idclient == id);
        }
    }
}
