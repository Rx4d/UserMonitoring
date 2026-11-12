using System.Net.Http.Json;

namespace Monitoring.Client.Agent;

public sealed class AgentRegistrationService(HttpClient httpClient)
{
    public async Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        var userCreationData = new UserCreation(
            AgentIdentity.GetOrCreateId(),
            new(Environment.UserName, Environment.UserDomainName, Environment.MachineName));

        using var response = await httpClient.PostAsJsonAsync("users", userCreationData, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}