using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unidash.Core.Infrastructure;
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

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddOpenApiDocument(settings => { settings.Title = "Unidash - Time Table API"; });

            services.AddOptions();
            services.Configure<TimeTableOptions>(Configuration.GetSection("TimeTable"));

            services.AddAutoMapper(coreAssembly);
            services.AddMediatR(coreAssembly);


            services.Configure<MongoDbConnectionOptions>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection") ?? "mongodb://mongodb";
                options.DatabaseName = "unidash_timetable";
            });
            services.AddSingleton(typeof(IEntityRepository<>), typeof(MongoEntityRepository<>));

            services.AddSingleton<ICalendarService, EntityCalendarService>();


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

            //app.UseHttpsRedirection();

            app.UseForwardedHeaders();

            app.UseRouting();

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
