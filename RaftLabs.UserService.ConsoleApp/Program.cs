using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using RaftLabs.UserService.DTO.Configuration;
using RaftLabs.UserService.Interface.Interface;
using RaftLabs.UserService.Persistence.UserService;
using System.Net.Http;




var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false);

    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ReqresApiOptionsDTO>(context.Configuration.GetSection("ReqresApi"));
        services.AddMemoryCache();
        services.AddHttpClient<IReqresClient, ReqresClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ReqresApiOptionsDTO>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        })
       .AddPolicyHandler(GetRetryPolicy()); // This works only if return type is IAsyncPolicy<HttpResponseMessage>


        services.AddLogging();
    })
    .Build();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));


var client = host.Services.GetRequiredService<IReqresClient>();

var user = await client.GetUserByIdAsync(2);
Console.WriteLine($"{user?.FirstName} {user?.LastName}");

var users = await client.GetAllUsersAsync();
foreach (var u in users)
{
    Console.WriteLine($"{u.Id}: {u.FirstName} {u.LastName}");
}
