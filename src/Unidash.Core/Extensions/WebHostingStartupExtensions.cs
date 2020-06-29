using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Unidash.Core.Utilities;

namespace Unidash.Core.Extensions
{
    public static class WebHostingStartupExtensions
    {
        public static IServiceCollection AddContextAccessors(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();
            services.AddSingleton<IUserIdProvider, HttpContextCurrentUserAccessor>();

            return services;
        }
    }
}
