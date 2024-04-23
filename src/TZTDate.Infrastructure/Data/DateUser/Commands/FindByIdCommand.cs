using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data.DateUser.Commands
{
    public class FindByIdCommand : IRequest<User>
    {
        public int Id { get; set; }
    }
}