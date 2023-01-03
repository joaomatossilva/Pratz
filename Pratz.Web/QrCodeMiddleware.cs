namespace Pratz.Web;

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QRCoder;

public class QrCodeMiddleware
{
    private readonly RequestDelegate _next;

    public QrCodeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var room = context.Request.Query["room"];
        
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode($"https://pratz.azurewebsites.net/Room/{room}", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        
        context.Response.Headers.Add("Content-Type", "image/png");
        var rawData = qrCode.GetGraphic(20);
        await context.Response.Body.WriteAsync(rawData, 0, rawData.Length);
    }
}