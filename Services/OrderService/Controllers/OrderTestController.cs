using MassTransit;
using MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderTestController : ControllerBase
    {
        private readonly ILogger<OrderTestController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderTestController(ILogger<OrderTestController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> SendMessage(string name, int age, string key)
        {
            await _publishEndpoint.Publish<People>(new
            {
                Name = name,
                Age = age,
                Key = key
            }
            //,x => x.SetRoutingKey("Order")
            );

            _logger.LogWarning("Message sended from Order");

            return Ok("Сообщение отправлено");
        }
    }
}