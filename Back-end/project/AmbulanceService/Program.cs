using AmbulanceService.DAL;
using Microsoft.EntityFrameworkCore;

namespace AmbulanceService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure database connection string
            builder.Services.AddDbContext<AmbulanceDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conStr")));

            // Add services to the container
            builder.Services.AddControllers();

            // CORS configuration to allow React app from a specific port (development)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")  // React app URL
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowReactApp");  // Use the "AllowReactApp" CORS policy

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}
