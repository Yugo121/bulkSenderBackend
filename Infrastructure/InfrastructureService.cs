using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class InfrastructureService
    {
        public InfrastructureService(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
                       .EnableSensitiveDataLogging()
            );

            services.AddScoped<IAppDbContext>(sp =>
                sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext()
            );

            services.AddScoped<ICsvProductParser, CsvProductParser>();
            services.AddHttpClient<IBaselinkerService, BaselinkerService>();
            services.AddHttpClient<INuboService, NuboService>();
        }

        //public InfrastructureService(IServiceCollection services, IConfiguration configuration)
        //{
        //    // 1) Connection string do pliku SQLite (zdefiniowany w appsettings.json)
        //    var connectionString = configuration.GetConnectionString("DefaultConnection");

        //    // 2) Rejestrujemy DbContextFactory z providerem SQLite
        //    services.AddDbContextFactory<AppDbContext>(options =>
        //        options.UseSqlite(connectionString)
        //               .EnableSensitiveDataLogging()
        //    );

        //    // 3) Rejestrujemy IAppDbContext tak jak wcześniej
        //    services.AddScoped<IAppDbContext>(sp =>
        //        sp.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext()
        //    );

        //    // … reszta Twoich serwisów bez zmian
        //    services.AddScoped<ICsvProductParser, CsvProductParser>();
        //    services.AddHttpClient<IBaselinkerService, BaselinkerService>();
        //    services.AddHttpClient<INuboService, NuboService>();
        //} - to jest do SQLite, w domu usunąć sql server, dodać paczkę nugetową z litem, na froncie dodać electrona i sprawdzić czy bedzie działać apka

//        {
//  "ConnectionStrings": {
//    "DefaultConnection": "Data Source=appdata.db"
//  }
//} - to jeszcze do appsettings wrzucić
}
}
