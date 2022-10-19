namespace Api.IntegrationTests;

public class IntegrationTestFixture
{
    public IntegrationTestFixture()
    {
        Factory = new ApiWebApplicationFactory<Startup>();
    }

    public ApiWebApplicationFactory<Startup> Factory { get; }
}