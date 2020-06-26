using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unidash.Core.Domain;

namespace Unidash.Auth.Application.Features.Users.Requests.Responses
{
    public class LoginUserResponse
    {
        public string AccessToken { get; set; }
    }
}
