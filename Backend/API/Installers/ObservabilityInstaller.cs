using Infrastructure.Data;

namespace API.Installers;

public class ObservabilityInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("db");
    }
}
