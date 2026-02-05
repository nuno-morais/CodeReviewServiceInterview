using CodeReviewServiceInterview.Models;
using External.AuditService;

namespace CodeReviewServiceInterview.Repositories;


class MessagesRepository
{
    // In-memory storage for messages
    private readonly List<Message> _whatsapp_messages = new();
    private readonly List<Message> _sms_messages = new();
    private readonly AuditService auditService = new AuditService();

    public IEnumerable<Message> GetMessages()
    {
        auditService.Log("GetMessages called");
        return _sms_messages.Select(m => new Message
        {
            Id = m.Id,
            From = m.From,
            To = m.To,
            Content = m.Content,
            Provider = "sms"
        }).Concat(_whatsapp_messages.Select(m => new Message
        {
            Id = m.Id,
            From = m.From,
            To = m.To,
            Content = m.Content,
            Provider = "whatsapp"
        }));
    }

    public Message AddMessage(string from, string to, string content, string provider)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            From = from,
            To = to,
            Content = content,
        };
      	if (provider == "sms") {
        	_sms_messages.Add(message);
        } else if (provider == "whatssapp"){
        	_whatsapp_messages.Add(message);
        }
        auditService.Log($"Message added: {message.Id}");

        return message;
    }
}