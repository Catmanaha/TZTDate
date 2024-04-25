namespace TZTDate.Core.Data.DateChat.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string Owner { get; set; }
    public int PrivateChatId { get; set; }
}