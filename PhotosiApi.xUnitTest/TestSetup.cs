using Bogus;

namespace PhotosiApi.xUnitTest;

public class TestSetup
{
    protected readonly Randomizer _faker;

    protected TestSetup()
    {
        _faker = new Randomizer();
    }
}