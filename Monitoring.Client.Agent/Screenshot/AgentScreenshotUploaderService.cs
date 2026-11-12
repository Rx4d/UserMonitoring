using System.Net.Http.Headers;

namespace Monitoring.Client.Agent;

public sealed class AgentScreenshotUploaderService(HttpClient httpClient)
{
    public async Task UploadAsync(Guid requestId)
    {
        try
        {
            var imageBytes = WindowsScreenCaptureService.TakeScreenshotAsBytes();
            if (imageBytes is null) return;

            using var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            using var response = await httpClient.PostAsync($"users/screenshots/{requestId}/upload", content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ScreenshotEX:{ex}");
        }
    }
}