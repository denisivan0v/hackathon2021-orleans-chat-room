using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<ClusterOptions>(options =>
        {
            options.ServiceId = "chat-room";
            options.ClusterId = "deivanov-cluster";
        });
    })
    .UseOrleans((context, siloBuilder) =>
    {
        siloBuilder
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
            .UseAzureStorageClustering(builder => builder.BindConfiguration("Clustering"))
            .AddAzureBlobGrainStorage("PubSubStore", builder => builder.BindConfiguration("GrainStorage"))
            .AddSimpleMessageStreamProvider("chat", options =>
            {
                options.FireAndForgetDelivery = true;
            });
    })
    .RunConsoleAsync();
