namespace TZTDate.Core.Data.DateChat.Entities;

using TZTDate.Core.Data.DateMessage.Entities;

public class Chat
{
    public int ChatId { get; set; }
    public List<User>? Participants { get; set; }
    public List<Message>? Messages { get; set; }
    public DateTime LastActivity { get; set; }
    public bool IsOpen { get; set; }
}
