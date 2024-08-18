using PhotosiApi.HttpClients;
using PhotosiApi.Service.AddressBook;
using PhotosiApi.Service.Order;
using PhotosiApi.Service.Product;
using PhotosiApi.Service.User;
using PhotosiApi.Service.User.Login;
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

        var appSettings = _builder.Configuration.GetSection("Settings");
        _builder.Services.Configure<AppSettings>(appSettings);
        
        ConfigureHttp();
        
        _builder.Services.AddAutoMapper(typeof(Startup));
        ConfigureMyServices(_builder.Services);
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
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IAddressBookService, AddressBookService>();

        // Singleton
        _ = services.AddSingleton<IUserLoginHandler, UserLoginHandler>();
    }
    
    private void ConfigureHttp()
    {
        _builder.Services.AddHttpClient<IBaseHttpClient, BaseHttpClient>();
    }
}