using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using PhotosiApi.Settings;

namespace PhotosiApi.xUnitTest;

public class TestSetup
{
    protected readonly Randomizer _faker;
    protected Mock<IOptions<AppSettings>> _mockAppSettings;

    protected TestSetup()
    {
        _faker = new Randomizer();

        _mockAppSettings = new Mock<IOptions<AppSettings>>();
        _mockAppSettings.Setup(x => x.Value).Returns(new AppSettings
        {
            PhotosiOrdersUrl = _faker.String2(15, 30),
            PhotosiProductsUrl = _faker.String2(15, 30),
            PhotosiUsersUrl = _faker.String2(15, 30),
            PhotosiAddressBooksUrl = _faker.String2(15, 30)
        });
    }
}