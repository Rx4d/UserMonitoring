using Monitoring.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR(options => options.MaximumReceiveMessageSize = Limits.MaxPayloadSize);
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

app.MapControllers();
app.MapHub<UserHub>("/hubs/users");

app.Run();