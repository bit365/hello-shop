using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloShop.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IdentityServiceDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return dbContext.Set<User>();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User? Get(int id)
        {
            return dbContext.Set<User>().Find(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] User value)
        {
            dbContext.Add(value);
            dbContext.SaveChanges();
        }
    }
}
