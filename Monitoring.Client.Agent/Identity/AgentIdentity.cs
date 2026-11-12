namespace Monitoring.Client.Agent;

public static class AgentIdentity
{
    private const string IdKey = "agent_identity_id";

    public static Guid GetOrCreateId()
    {
        var value = Preferences.Get(IdKey, string.Empty);

        if (Guid.TryParse(value, out var id) && id != Guid.Empty)
            return id;

        id = Guid.NewGuid();
        Preferences.Set(IdKey, id.ToString());

        return id;
    }
}