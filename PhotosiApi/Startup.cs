﻿using PhotosiApi.HttpClients.User;
using PhotosiApi.Security;
using PhotosiApi.Service.User;
using PhotosiApi.Settings;

namespace PhotosiApi;

public class Startup
{
    private readonly WebApplicationBuilder _builder;

    public Startup(WebApplicationBuilder builder)
    {
        _builder = builder;
    }

    public void ConfigureServices()
    {
        _ = _builder.Services.AddControllers();

        var appSettings = _builder.Configuration.GetSection("Settings").Get<AppSettings>();
        ConfigureSettings(appSettings);
        ConfigureHttp(appSettings);
        
        _builder.Services.AddAutoMapper(typeof(Startup));
        ConfigureMyServices(_builder.Services);
    }
    
    private void ConfigureSettings(AppSettings appSettings)
    {
        _ = _builder.Services.AddSingleton(appSettings);
    }
    
    public void Configure(IApplicationBuilder app)
    {
        _ = app.UseRouting();
        _ = app.UseAuthorization();
        _ = app.UseEndpoints(x => x.MapControllers());
    }

    private void ConfigureMyServices(IServiceCollection services)
    {
        // Aggiunge i propri servizi al container di dependency injection.
        _ = services.AddScoped<IUserService, UserService>()
            ;

        // Singleton
        _ = services.AddSingleton<IUserLoginHandler, UserLoginHandler>();
    }
    
    private void ConfigureHttp(AppSettings appSettings)
    {
        _builder.Services.AddHttpClient<IUserHttpClient, UserHttpClient>();
    }
}