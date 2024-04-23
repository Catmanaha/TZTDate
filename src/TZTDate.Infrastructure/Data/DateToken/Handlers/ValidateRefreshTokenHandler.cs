using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateToken.Responses;
using TZTDate.Infrastructure.Data.DateToken.Commands;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class ValidateRefreshTokenHandler : IRequestHandler<ValidateRefreshTokenCommand, RefreshTokenValidationResponse>
{
    private readonly TZTDateDbContext context;

    public ValidateRefreshTokenHandler(TZTDateDbContext context)
    {
        this.context = context;
    }

    public async Task<RefreshTokenValidationResponse> Handle(ValidateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await this.context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.Token);

        if (refreshToken == null)
        {
            return new RefreshTokenValidationResponse
            {
                IsValid = false,
                Message = "Token does not exist"
            };
        }

        if (refreshToken.UserId != request.UserId)
        {
            return new RefreshTokenValidationResponse
            {
                Message = "Token does not belong to this user"
            };
        }

        if (refreshToken.Revoked)
        {
            return new RefreshTokenValidationResponse
            {
                Message = "Token has been revoked"
            };
        }

        if (refreshToken.ExpiryDate < DateTime.UtcNow)
        {
            return new RefreshTokenValidationResponse
            {
                IsValid = false,
                Message = "Token has expired"
            };
        }

        return new RefreshTokenValidationResponse
        {
            IsValid = true
        };
    }
}
