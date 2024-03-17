namespace TZTDate.Core.Data.DateMessage;    
public class Message
{
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string? MessageText { get; set; }
    public DateTime Timestamp { get; set; }
}
