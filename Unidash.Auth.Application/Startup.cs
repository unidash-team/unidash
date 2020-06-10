using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Unidash.Auth.Users.Commands;
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
            services.Configure<MongoDbConnectionOptions>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection") ?? "mongodb://mongodb";
                options.DatabaseName = "unidash_auth";
            });

            services.AddControllers();
            services.AddOpenApiDocument(settings => { settings.Title = "Unidash - Auth API"; });

            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("GlobalAdmin", policy => policy.RequireRole("GlobalAdmin"));
                options.AddPolicy("CourseAdmin", policy => policy.RequireRole("CourseAdmin"));
            });

            services.AddMediatR(typeof(CreateUserCommand).Assembly);
            services.AddAutoMapper(typeof(CreateUserCommand).Assembly);


            services.AddSingleton(typeof(IEntityRepository<>), typeof(MongoEntityRepository<>));

            services.AddSingleton<PasswordService>();
            services.AddSingleton<JwtTokenService>(provider => new JwtTokenService(unidashSecurityKey));
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

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
