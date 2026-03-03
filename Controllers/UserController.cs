using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.User.Api.Data;

namespace Library.User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. HÄMTA ALLA
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // 2. HÄMTA EN SPECIFIK
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("Användaren hittades inte.");
            return Ok(user);
        }

        // 3. SKAPA (Använder din AddUserDto)
        [HttpPost]
        public async Task<IActionResult> AddUser(Library.User.Api.Models.AddUserDto addUserDto)
        {
            var userEntity = new Library.User.Api.Models.Entities.UserEntity()
            {
                Name = addUserDto.Name,
                LastName = addUserDto.LastName,
                Email = addUserDto.Email,
                EmployeeId = addUserDto.EmployeeId,
                PasswordHash = addUserDto.PasswordHash
            };

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return Ok(userEntity);
        }

        // 4. UPPDATERA (Använder din UpdateUser)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Library.User.Api.Models.UpdateUser updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null) return NotFound("Användaren finns inte.");

            user.Name = updateDto.Name;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;
            user.EmployeeId = updateDto.EmployeeId;
            user.PasswordHash = updateDto.PasswordHash;

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // 5. RADERA (Extra bonus så du är helt klar!)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Användare raderad.");
        }
    }
}