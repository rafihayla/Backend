using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace myAPI.Controllers
{
    [Route("api/order"), Authorize]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_orderService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_orderService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] OrderCreateDto value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_orderService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            await _orderService.AddOrderAsync(Guid.Parse(userId), value);
            return Ok();
        }

        // POST api/<OrderController>/5/status
        [HttpPost("{id}/status")]
        public ActionResult<Response> Confirm(Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (_orderService.IsAuthorized(identity) == false)
            {
                return Unauthorized();
            }
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if(userId == null)
            {
                return Unauthorized();
            }
            return _orderService.ConfirmOrderAsync(Guid.Parse(userId), id);
        }
    }
}
