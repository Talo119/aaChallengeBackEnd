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
using Microsoft.Extensions.Configuration;

namespace AAchallenge.Web.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbContextAAchallenge _context;
        private readonly IConfiguration _config;

        public UsersController(DbContextAAchallenge context, IConfiguration config)
        {
            _context = context;
            _config = config;
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

        //POST : api/Users/Create
        [HttpPost]
        [ActionName("Create")]
        public async Task<ActionResult> Create ([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var email = model.email.ToLower();
            if(await _context.Users.AnyAsync(u=> u.email == email))
            {
                return BadRequest("The email already exists.");
            }

            CreatePasswordHash(model.password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                idrole = model.idrole,
                nombre = model.nombre,
                email = model.email,
                password_hash = passwordHash,
                password_salt = passwordSalt,
                condicion = true
            };

            _context.Users.Add(user);

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


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.iduser == id);
        }
    }
}
