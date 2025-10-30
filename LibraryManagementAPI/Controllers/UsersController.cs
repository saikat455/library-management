using Bogus;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private const string UsersCacheKey = "AllUsers";

        public UsersController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is required");

            user.TimeStamp = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _cache.Remove(UsersCacheKey);

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        [HttpPost("create-bulk-users")]
        public async Task<IActionResult> CreateBulkUsers()
        {
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Age, f => f.Random.Int(18, 80))
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.TimeStamp, f => f.Date.Between(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow));

            var users = userFaker.Generate(10000);

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            _cache.Remove(UsersCacheKey);

            return Ok(new { message = "10,000 users created successfully", count = users.Count });
        }

        [HttpGet("fetch-users")]
        public async Task<IActionResult> GetUsers()
        {
            if (!_cache.TryGetValue(UsersCacheKey, out List<User>? users))
            {
                users = await _context.Users.ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(UsersCacheKey, users, cacheOptions);
            }

            return Ok(new
            {
                count = users!.Count,
                data = users,
                cached = _cache.TryGetValue(UsersCacheKey, out _)
            });
        }

        [HttpGet("clear-cache")]
        public IActionResult ClearCache()
        {
            _cache.Remove(UsersCacheKey);
            return Ok(new { message = "Cache cleared successfully" });
        }
    }
}