namespace Api.IntegrationTests;

[Collection("Api")]
public abstract class IntegrationTest
{
    private readonly HttpClient _client;

    protected IntegrationTest(IntegrationTestFixture fixture)
    {
        Factory = fixture.Factory;
        _client = Factory.CreateClient();
    }

    private ApiWebApplicationFactory<Startup> Factory { get; }

    protected async Task<HttpResponseMessage> Get(string url)
    {
        return await _client.GetAsync(url);
    }
}