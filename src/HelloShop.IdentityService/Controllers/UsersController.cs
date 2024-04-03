using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.IdentityService.Models.Users;
using HelloShop.ServiceDefaults.Authorization;
using HelloShop.ServiceDefaults.Models.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloShop.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IdentityServiceDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<ActionResult<PagedResponse<UserListItem>>> GetUsers([FromQuery] UserListRequest model)
        {
            var userList= await dbContext.Set<User>().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

            var result = userList.Select(e => new UserListItem
            {
                Id = e.Id,
                UserName = e.UserName!,
                PhoneNumber = e.PhoneNumber,
                PhoneNumberConfirmed = e.PhoneNumberConfirmed,
                Email = e.Email,
                EmailConfirmed = e.EmailConfirmed,
                CreationTime = e.CreationTime,
            }).ToList();

            var responseModel = new PagedResponse<UserListItem>(result, result.Count);

            return Ok(responseModel);
        }

        [HttpGet("{id}")]
        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<ActionResult<UserDetailsResponse>> GetUser(int id, [FromServices] IAuthorizationService authorizationService)
        {
            ResourceInfo resource = new(nameof(User), id.ToString());

            var authorizationResult = await authorizationService.AuthorizeAsync(User, resource, IdentityPermissions.Users.Default);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            User? user = dbContext.Set<User>().Find(id);

            if (user == null)
            {
                return NotFound();
            }

            UserDetailsResponse responseModel = new()
            {
                Id = user.Id,
                UserName = user.UserName!,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                CreationTime = user.CreationTime,
            };

            return Ok(responseModel);
        }

        [HttpPost]
        [Authorize(IdentityPermissions.Users.Create)]
        public async Task<ActionResult<UserDetailsResponse>> PostUser([FromBody] UserCreateRequest model)
        {
            var user = new User
            {
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            UserDetailsResponse responseModel = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                CreationTime = user.CreationTime,
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseModel);
        }


        [HttpPut("{id}")]
        [Authorize(IdentityPermissions.Users.Update)]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserUpdateRequest model)
        {
            if (model.Id != id)
            {
                throw new ArgumentException("Id mismatch", nameof(model));
            }

            var user = dbContext.Set<User>().Find(id);

            if (user != null)
            {
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
            }

            await dbContext.SaveChangesAsync();

            return Ok();
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

        [HttpDelete]
        [Authorize(IdentityPermissions.Users.Delete)]
        public async Task<IActionResult> DeleteUsers(IEnumerable<int> ids)
        {
            await dbContext.Set<User>().Where(e => ids.Contains(e.Id)).ExecuteDeleteAsync();

            return NoContent();
        }
    }
}
