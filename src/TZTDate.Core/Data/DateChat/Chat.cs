namespace TZTDate.Core.Data.DateChat;

using TZTDate.Core.Data.DateMessage;

public class Chat
{
    public int ChatId { get; set; }
    public List<User> Participants { get; set; }
    public List<Message> Messages { get; set; }
    public DateTime LastActivity { get; set; }
    public bool IsOpen { get; set; }
}
