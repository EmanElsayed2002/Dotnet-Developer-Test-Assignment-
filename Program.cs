
using E_Technology_Task.Services;
using Microsoft.OpenApi.Models;

namespace E_Technology_Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Country Block API", Version = "v1" });
            });

            builder.Services.AddHttpClient<IGeoLocationService, GeoLocationService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["IpApi:BaseUrl"]);
            });

            builder.Services.AddSingleton<ICountryBlockService, CountryBlockService>();
            builder.Services.AddHostedService<TemporalBlockCleanupService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
