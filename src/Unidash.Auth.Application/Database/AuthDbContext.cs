using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Unidash.Auth.Application.DataModels;

namespace Unidash.Auth.Application.Database
{
    public class AuthDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AuthDbContext()
        {
        }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
