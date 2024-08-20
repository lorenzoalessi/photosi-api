using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        Startup startup = new Startup(builder);
        startup.ConfigureServices();

        // Costruisci l'applicazione Web.
        WebApplication app = builder.Build();

        // Configura l'applicazione Web.
        startup.Configure(app);

        // Avvia l'applicazione.
        await app.RunAsync();
    }
}