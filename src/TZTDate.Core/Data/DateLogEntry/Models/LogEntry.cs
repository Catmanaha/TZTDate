namespace TZTDate.Core.Data.DateLogEntry.Models;

public class LogEntry
{
    public int Id { get; set; }
    public DateTime EventDate { get; set; }
    public string EventIp { get; set; }
    public int EventUserId { get; set; }
    public string EventType { get; set; }
}
