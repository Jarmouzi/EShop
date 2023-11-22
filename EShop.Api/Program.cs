
using EShop.DataContext;
using EShop.IdentityService.Infrastructure;
using EShop.Infrastructure;
using EShop.LogService.DataContext;
using EShop.LogService.Repository;
using EShop.Model;

namespace EShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationJwtAuth(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());
            builder.Services.AddApplicationAuthorization();

            builder.Services.AddApplicationServices<EShopLogContext>(builder.Configuration.GetConnectionString("LogConnection"));
            builder.Services.AddScoped<LogService.DataContext.IUnitOfWork, LogService.DataContext.UnitOfWork>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            builder.Services.AddApplicationServices<EShopContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddScoped<DataContext.IUnitOfWork, DataContext.UnitOfWork>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}