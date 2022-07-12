using GenericRepository.Data;
using GenericRepository.Interfaces;
using GenericRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using ReportesApp.Services;

namespace ReportesApp;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Local"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<AppDbContext>());        
        services.AddTransient(typeof(IReporteRepository), typeof(ReporteRepository));
        services.AddTransient(typeof(IReportesService), typeof(ReportesService));

        return services;
    }
}
