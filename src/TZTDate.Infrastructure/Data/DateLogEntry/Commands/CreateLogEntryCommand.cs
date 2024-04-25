using MediatR;
using TZTDate.Core.Data.DateLogEntry.Models;

namespace TZTDate.Infrastructure.Data.DateLogEntry.Commands;

public class CreateLogEntryCommand : IRequest
{
    public LogEntry LogEntry { get; set; }
}
