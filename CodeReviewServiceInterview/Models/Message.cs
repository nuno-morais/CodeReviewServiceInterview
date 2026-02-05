namespace CodeReviewServiceInterview.Models;

public class Message
{
    public Guid Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Content { get; set; }
    public string Provider { get; set; }
}
