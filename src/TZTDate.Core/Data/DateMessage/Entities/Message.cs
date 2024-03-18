namespace TZTDate.Core.Data.DateMessage.Entities;    
public class Message
{
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string? MessageText { get; set; }
    public DateTime Timestamp { get; set; }
}

public class MyMessage
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Text { get; set; }
    public DateTime Timestamp { get; set; }
}