using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Settings;

[ExcludeFromCodeCoverage]
public class AppSettings
{
    public string PhotosiOrdersUrl { get; set; }

    public string PhotosiProductsUrl { get; set; }

    public string PhotosiUsersUrl { get; set; }

    public string PhotosiAddressBooksUrl { get; set; }
}