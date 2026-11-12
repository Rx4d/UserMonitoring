using System.Drawing;

namespace Monitoring.Client.Agent;

public static class WindowsScreenCaptureService
{
    public static byte[] TakeScreenshotAsBytes()
    {
        var display = DeviceDisplay.Current.MainDisplayInfo;
        var screenWidth = (int)display.Width;
        var screenHeight = (int)display.Height;

        using var fullBitmap = new Bitmap(screenWidth, screenHeight);
        using (var graphics = Graphics.FromImage(fullBitmap))
        {
            graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));
        }

        var targetWidth = Math.Min(screenWidth, 1680);
        var targetHeight = (int)(screenHeight * ((double)targetWidth / screenWidth));

        using var resizedBitmap = new Bitmap(fullBitmap, new System.Drawing.Size(targetWidth, targetHeight));
        using var stream = new MemoryStream();

        resizedBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        return stream.ToArray();
    }
}