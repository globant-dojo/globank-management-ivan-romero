using ClientesApp.Services;
using GenericRepository.Data;
using GenericRepository.Interfaces;
using GenericRepository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClientesApp;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Local"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddSingleton(typeof(IClientesService), typeof(ClientesService));
        services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient(typeof(IReadRepository<>), typeof(BaseRepository<>));
        //services.AddTransient<ICustomPostRepository, CustomPostRepository>();

        return services;
    }
}
