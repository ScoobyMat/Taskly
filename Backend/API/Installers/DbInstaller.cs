using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Installers;

public class DbInstaller : IInstaller
    {
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
