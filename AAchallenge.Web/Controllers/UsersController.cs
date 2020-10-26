using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities.Users;
using AAchallenge.Web.Models.Users.User;

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;

        public UsersController(DbContextAAchallenge context)
        {
            _context = context;
        }

        //GET : api/Users/List
        [HttpGet]
        [ActionName("List")]
        public async Task<IEnumerable<UserViewModel>> List()
        {
            var users = await _context.Users.Include(u => u.role).ToListAsync();
            return users.Select(u => new UserViewModel
            {
                iduser = u.iduser,
                idrole = u.idrole,
                role = u.role.name,
                nombre = u.nombre,                
                email = u.email,
                password_hash = u.password_hash,
                condicion = u.condicion
            });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.iduser == id);
        }
    }
}
