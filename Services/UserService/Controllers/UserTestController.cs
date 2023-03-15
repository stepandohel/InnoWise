using MassTransit;
using MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTestController : ControllerBase
    {
        private readonly ILogger<UserTestController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        public UserTestController(ILogger<UserTestController> logger, IPublishEndpoint publishEndpoint)
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
            //, x => x.SetRoutingKey("User")
            );

            _logger.LogWarning("Message sended from UserService");

            return Ok("Сообщение отправлено");
        }
    }
}