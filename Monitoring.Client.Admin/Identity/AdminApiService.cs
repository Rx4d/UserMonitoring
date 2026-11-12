using Monitoring.Identity.Contracts;
using System.Collections.Immutable;
using System.Net.Http.Json;

namespace Monitoring.Client.Admin;

public sealed class AdminApiService(HttpClient httpClient)
{
    public async Task<ImmutableArray<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await httpClient.GetFromJsonAsync<ImmutableArray<User>>("users", cancellationToken);

        return users;
    }

    public async Task<Guid> RequestScreenshotAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsync($"users/screenshots/{userId}/request", null, cancellationToken);

        response.EnsureSuccessStatusCode();

        var requestId = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken: cancellationToken);
        return requestId;
    }
}