using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using GreenPipes;
using MassTransit;
using NSwag.AspNetCore;
using Unidash.Auth.Application.Database;
using Unidash.Auth.Application.DataModels;
using Unidash.Auth.Application.Features.Users.Requests;
using Unidash.Core.Auth;
using Unidash.Core.Extensions;
using Unidash.Core.Infrastructure;
using Unidash.Core.Security;

namespace Unidash.Auth.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers();
            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "Unidash - Auth API";
            });

            services.AddHttpContextAccessor();

            services.AddDbContext<AuthDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("AuthDbContext")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddUnidashAuthentication(Configuration.GetSection("Auth"));

            services.AddMediatR(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);
            services.AddMessageBroker(GetType());

            services.AddContextAccessors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseOpenApi();
            app.UseSwaggerUi3(config =>
            {
                config.SwaggerRoutes.Add(new SwaggerUi3Route("v1 - Gateway", "/auth/swagger/v1/swagger.json"));
                config.SwaggerRoutes.Add(new SwaggerUi3Route("v1", "/swagger/v1/swagger.json"));
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
