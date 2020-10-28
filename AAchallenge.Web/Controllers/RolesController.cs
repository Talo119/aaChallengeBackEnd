using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using AAchallenge.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;

        public RolesController(DbContextAAchallenge context)
        {
            _context = context;
        }

        //GET : api/Roles/List
        [HttpGet]
        [ActionName("List")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<RoleViewModel>> List()
        {
            var role = await _context.Roles.ToListAsync();
            return role.Select(r => new RoleViewModel
            {
                idrole = r.idrole,
                name = r.name,
                description = r.description,
                condicion = r.condicion
            });
        }

        //GET : api/Roles/Select
        [HttpGet]
        [ActionName("Select")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var role = await _context.Roles.Where(r => r.condicion == true).ToListAsync();
            return role.Select(r => new SelectViewModel
            {
                idrole = r.idrole,
                name = r.name
            });
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.idrole == id);
        }
    }
}
