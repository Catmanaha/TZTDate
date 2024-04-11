using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Services.Base
{
    public interface ITokenService
    {
        public string CreateToken(User user);
        public string CreateTokenAdmin(User user);
    }
}