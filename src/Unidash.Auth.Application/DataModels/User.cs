using Microsoft.AspNetCore.Identity;
using System;

namespace Unidash.Auth.Application.DataModels
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
