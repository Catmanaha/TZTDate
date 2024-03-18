using TZTBank.Infrastructure.Data.BankUser.Handlers;

namespace TZTDate.UnitTests.Data.DateUser.Handlers;

public class AddNewHandlerTest
{
    [Fact]
    public async Task Handle_UserDtoNull_ThrowNullReferenceException()
    {
        var handler = new AddNewHandler(null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, new CancellationToken()));
    }
}
