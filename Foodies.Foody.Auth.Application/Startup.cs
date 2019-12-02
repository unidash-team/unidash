using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using AutoMapper;
using Foodies.Foody.Auth.Commands;
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
            services.AddControllers();
            services.AddOpenApiDocument();

            services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddDiscord(options =>
                {
                    options.ClientId = Configuration.GetSection("Foody:Auth:Providers:Discord:ClientId").Value;
                    options.ClientSecret = Configuration.GetSection("Foody:Auth:Providers:Discord:ClientSecret").Value;
                    options.Scope.Add("identify");
                    options.Scope.Add("email");

                    options.SaveTokens = true;
                })
                .AddCookie("Bearer");

            services.AddMediatR(typeof(CreateUserCommand).Assembly);
            services.AddAutoMapper(typeof(CreateUserCommand).Assembly);

            // TODO: Use production DB
            services.AddSingleton(typeof(IEntityRepository<>), typeof(InMemoryEntityRepository<>));

            services.AddSingleton<JwtTokenService>(provider =>
                new JwtTokenService(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Configuration.GetSection("Foody:Auth:SecurityKey").Value ?? "verysecureindeed!123"))));
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

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
