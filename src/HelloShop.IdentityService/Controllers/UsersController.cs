using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloShop.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IdentityServiceDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        [Authorize(IdentityPermissions.Users.Default)]
        public IEnumerable<User> GetUsers()
        {
            return dbContext.Set<User>();
        }

        [HttpGet("{id}")]
        [Authorize(IdentityPermissions.Users.Default)]
        public User? GetUser(int id)
        {
            return dbContext.Set<User>().Find(id);
        }

        [HttpPost]
        [Authorize(IdentityPermissions.Users.Create)]
        public void PostUser([FromBody] User value)
        {
            dbContext.Add(value);
            dbContext.SaveChanges();
        }

        [HttpPut("{id}")]
        [Authorize(IdentityPermissions.Users.Update)]
        public void PutUser(int id, [FromBody] User value)
        {
            var user = dbContext.Set<User>().Find(id);
            if (user != null)
            {
                dbContext.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(IdentityPermissions.Users.Delete)]
        public async Task<IActionResult> DeleteUser(int id, [FromServices] IAuthorizationService authorizationService)
        {
            var user = dbContext.Set<User>().Find(id);

            if (user != null)
            {
                var result = await authorizationService.AuthorizeAsync(User, user, IdentityPermissions.Users.Delete);

                if (result.Succeeded)
                {
                    dbContext.Remove(user);

                    dbContext.SaveChanges();

                    return Ok();
                }
            }

            return Unauthorized();
        }

        [HttpGet(nameof(Bar))]
        [Authorize(IdentityPermissions.Users.Create)]
        public IActionResult Bar()
        {
            return Ok("Hello, World!");
        }
    }
}
