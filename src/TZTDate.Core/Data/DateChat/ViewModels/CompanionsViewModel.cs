using TZTDate.Core.Data.DateChat.Entities;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Core.Data.DateChat.ViewModels;

public class CompanionsViewModel
{
    public User? CurrentUser { get; set; }
    public PrivateChat PrivateChat { get; set; }
}
