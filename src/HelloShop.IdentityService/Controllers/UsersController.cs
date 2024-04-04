using AutoMapper;
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
    public class UsersController(IdentityServiceDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        [Authorize(IdentityPermissions.Users.Default)]
        public async Task<ActionResult<PagedResponse<UserListItem>>> GetUsers([FromQuery] UserListRequest model)
        {
            var userList = await dbContext.Set<User>().Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

            var result = mapper.Map<IReadOnlyList<UserListItem>>(userList);

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

            User? user = await dbContext.Set<User>().FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return mapper.Map<UserDetailsResponse>(user);
        }

        [HttpPost]
        [Authorize(IdentityPermissions.Users.Create)]
        public async Task<ActionResult<UserDetailsResponse>> PostUser([FromBody] UserCreateRequest model)
        {
            var user = mapper.Map<User>(model);

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var responseModel = mapper.Map<UserDetailsResponse>(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseModel);
        }


        [HttpPut("{id}")]
        [Authorize(IdentityPermissions.Users.Update)]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserUpdateRequest model)
        {
            if (model.Id != id)
            {
                return BadRequest();
            }

            var user = mapper.Map<User>(model);

            DbSet<User> users = dbContext.Set<User>();

            users.Entry(user).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!users.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(IdentityPermissions.Users.Delete)]
        public async Task<IActionResult> DeleteUser(int id, [FromServices] IAuthorizationService authorizationService)
        {
            var user = await dbContext.Set<User>().FindAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(User, user, IdentityPermissions.Users.Delete);

            if (authorizationResult.Succeeded)
            {
                dbContext.Remove(user);

               await dbContext.SaveChangesAsync();

                return NoContent();
            }

            return Forbid();
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
