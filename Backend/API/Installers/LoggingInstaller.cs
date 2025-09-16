namespace API.Installers;

public class LoggingInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSimpleConsole(o =>
            {
                o.SingleLine = true;
                o.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ ";
                o.IncludeScopes = true;
            });
        });
    }
}
