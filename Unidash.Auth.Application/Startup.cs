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
using Unidash.Auth.Application.Database;
using Unidash.Auth.Application.DataModels;
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
            var unidashSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration.GetSection("Unidash:Auth:SecurityKey").Value ??
                                       "verysecureindeed!123"));

            services.AddOptions();

            services.AddControllers();
            services.AddOpenApiDocument(settings => { settings.Title = "Unidash - Auth API"; });

            services.AddHttpContextAccessor();

            services.AddDbContext<AuthDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("AuthDbContext")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = "unidash",
                        ValidateAudience = true,

                        ValidIssuer = "unidash",
                        ValidateIssuer = true,

                        IssuerSigningKey = unidashSecurityKey,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };

                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                })
                .AddCookie();

            services.AddMediatR(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);

            services.AddSingleton<JwtTokenService>(provider => new JwtTokenService(unidashSecurityKey));
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
            app.UseSwaggerUi3();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
