using AutoMapper;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unidash.Chat.Application.Consumers;
using Unidash.Chat.Application.Hubs;
using Unidash.Chat.Application.Services;
using Unidash.Core.Auth;
using Unidash.Core.Extensions;

namespace Unidash.Chat.Application
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddOpenApiDocument();

            services.AddEntityRepository(Configuration.GetSection("Connections:MongoDb"));
            services.AddUnidashAuthentication(Configuration.GetSection("Auth"));
            services.AddContextAccessors();

            services.AddMediatR(GetType().Assembly);
            services.AddAutoMapper(GetType().Assembly);
            services.AddMessageBroker(GetType());

            services.AddSingleton<IMessageService, MessageService>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
