using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TZTDate.Core.Data.DateUser.Dtos
{
    public class UpdateTokenDto
    {
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; }
        public string IpAddress { get; set; }

    }
}