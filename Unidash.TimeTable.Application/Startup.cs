using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unidash.TimeTable.Options;
using Unidash.TimeTable.Services;
using Unidash.TimeTable.Workers;

namespace Unidash.TimeTable.Application
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
            var coreAssembly = typeof(MappingProfile).Assembly;

            services.AddControllers();
            services.AddOpenApiDocument(settings => { settings.Title = "Unidash - Time Table API"; });

            services.AddOptions();
            services.Configure<TimeTableOptions>(Configuration.GetSection("TimeTable"));

            services.AddAutoMapper(coreAssembly);
            services.AddMediatR(coreAssembly);

            services.AddSingleton<ICalendarService, EntityCalendarService>();

            services.AddDbContext<TimeTableDbContext>(builder => builder.UseSqlServer(
                Configuration.GetConnectionString("TimeTableDbContext"), b => { b.MigrationsAssembly(GetType().Namespace); }));

            services.AddHostedService<CalendarSyncBackgroundWorker>();


            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
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

            app.UseForwardedHeaders();

            app.UseAuthorization();

            app.UseOpenApi()
                .UseSwaggerUi3()
                .UseReDoc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
