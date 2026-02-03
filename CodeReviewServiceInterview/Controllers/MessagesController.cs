using CodeReviewServiceInterview.DTOs;
using CodeReviewServiceInterview.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeReviewServiceInterview.Controllers;

[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(ILogger<MessagesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("getmessages", Name = "GetMessages")]
    public IActionResult Get()
    {
        _logger.LogInformation("Received request to get messages");
        return Ok(MessagesService.GetMessages().Select(m => new MessageDTO
        {
            Id = m.Id,
            From = m.From,
            To = m.To,
            Content = m.Content
        }));
    }

    [HttpPost("createmessage", Name = "CreateMessage")]
    public IActionResult Post([FromBody] createMessageDTO message)
    {
        _logger.LogInformation("Received request to create a message: {Message}", message);
        return Accepted(MessagesService.Create_Message(message.From, message.To, message.Content, message.Provider));
    }
}
