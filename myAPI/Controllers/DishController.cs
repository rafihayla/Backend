using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace myAPI.Controllers
{
    [Route("api/dish")]
    [ApiController]
    public class DishController(IDishService dishService) : ControllerBase
    {
        private readonly IDishService _dishService = dishService;

        // GET: api/<DishController>
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<DishPagination>> Get([FromQuery]DishCategory[]? categories, bool? vegetarian, DishSorting? sorting, int? page)
        {
            return await _dishService.GetDishesAsync(categories, vegetarian, sorting, page);
        }

        // GET api/<DishController>/5
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<DishDto?> Get(Guid id)
        {
            return await _dishService.GetDishDtoByIdAsync(id);
        }

        // GET api/<DishController>/5/rating/check
        [HttpGet("{id}/rating/check")]
        public async Task<ActionResult<bool>> CheckRating(Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_dishService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return false;
            }
            return Ok(await _dishService.CanGiveRatingAsync(Guid.Parse(userId), id));
        }

        // POST api/<DishController>/5/rating
        [HttpPost("{id}/rating")]
        public async Task<ActionResult<Response>> Post(Guid id, [FromBody] int rating)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_dishService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }
            return await _dishService.AddRatingAsync(Guid.Parse(userId), id, rating);
        }
    }
}
