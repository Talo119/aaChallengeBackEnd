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
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
                email = model.email.ToLower(),
                condicion = true,
                password_hash = passwordHash,
                password_salt = passwordSalt
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

        //POST : api/Users/Login
        [HttpPost]
        [ActionName("Login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var email = model.email.ToLower();
            var user = await _context.Users.Where(u => u.condicion == true).Include(u => u.role).FirstOrDefaultAsync(u => u.email == email);

            if(user == null)
            {
                return NotFound();
            }

            if(!verifyPasswordHash(model.password, user.password_hash, user.password_salt))
            {
                return NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.iduser.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, user.role.name),
                new Claim("iduser", user.iduser.ToString()),
                new Claim("role", user.role.name),
                new Claim("nombre", user.nombre)

            };

            return Ok(
                new {token = GenerateToken(claims)}
            );
        }

        private bool verifyPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo));
            }
        }

        private string GenerateToken(List<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private object SymmetricSecurityKey(object p)
        {
            throw new NotImplementedException();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.iduser == id);
        }
    }
}
