using API.Installers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.InstallServicesInAssembly(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpMetrics();
app.MapMetrics("/api/metrics");

app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taskly API v1"); c.RoutePrefix = "swagger"; });

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/api/health");
app.MapHealthChecks("/api/ready");

app.MapControllers();

app.Run();
