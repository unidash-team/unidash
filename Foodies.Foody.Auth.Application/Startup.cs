using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using AutoMapper;
using Foodies.Foody.Auth.Users.Commands;
using Foodies.Foody.Core.Infrastructure;
using Foodies.Foody.Core.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Foodies.Foody.Auth.Application
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
            var foodySecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration.GetSection("Foody:Auth:SecurityKey").Value ??
                                       "verysecureindeed!123"));

            services.AddOptions();
            services.Configure<MongoDbConnectionOptions>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection") ?? "mongodb://mongodb";
                options.DatabaseName = "foody_auth";
            });

            services.AddControllers();
            services.AddOpenApiDocument(settings => { settings.Title = "foody Auth API"; });

            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration.GetSection("Foody:Auth:Providers:Discord:ClientId").Value;
                    options.ClientSecret = Configuration.GetSection("Foody:Auth:Providers:Discord:ClientSecret").Value;
                    options.Scope.Add("identify");
                    options.Scope.Add("email");

                    options.SaveTokens = true;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = "foody",
                        ValidateAudience = true,

                        ValidIssuer = "foody",
                        ValidateIssuer = true,

                        IssuerSigningKey = foodySecurityKey,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };

                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                })
                .AddCookie();

            services.AddMediatR(typeof(CreateUserCommand).Assembly);
            services.AddAutoMapper(typeof(CreateUserCommand).Assembly);


            services.AddSingleton(typeof(IEntityRepository<>), typeof(MongoEntityRepository<>));

            services.AddSingleton<PasswordService>();
            services.AddSingleton<JwtTokenService>(provider => new JwtTokenService(foodySecurityKey));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseOpenApi().UseSwaggerUi3();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
