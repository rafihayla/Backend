using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace myAPI.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController(IBasketService basketService) : ControllerBase
    {
        private readonly IBasketService _basketService = basketService;

        // GET: api/<BasketController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishBasketDto>>> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(_basketService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _basketService.GetBasketsAsync(Guid.Parse(userId??"")));
        }

        // POST api/<BasketController>
        [HttpPost("dish/{dishId}")]
        public async Task<ActionResult> Post(Guid dishId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_basketService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            await _basketService.AddBasketAsync(Guid.Parse(userId), dishId);
            return Ok();
        }

        // DELETE api/<BasketController>/5
        [HttpDelete("dish/{dishId}")]
        public async Task<ActionResult> Delete(Guid dishId, [FromQuery] bool increase = false)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_basketService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            await _basketService.DeleteBasketByIdAsync(Guid.Parse(userId), dishId, increase);
            return Ok();
        }
    }
}
