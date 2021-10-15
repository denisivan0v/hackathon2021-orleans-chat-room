using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;

await Host.CreateDefaultBuilder(args)
    .UseOrleans((context, siloBuilder) =>
    {
        siloBuilder
            .UseAzureStorageClustering(builder => builder.BindConfiguration("Clustering"))
            .UseLinuxEnvironmentStatistics()
            .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
            .UseDashboard()
            .AddAzureBlobGrainStorage("PubSubStore", builder => builder.BindConfiguration("GrainStorage"))
            .AddSimpleMessageStreamProvider("chat", options =>
            {
                options.FireAndForgetDelivery = true;
            });
        
        if (context.HostingEnvironment.IsDevelopment())
        {
            siloBuilder
                .Configure<ClusterOptions>(options =>
                {
                    options.ServiceId = "chat-room";
                    options.ClusterId = "deivanov-cluster";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback);
        }
        else
        {
            siloBuilder.UseKubernetesHosting();
        }
    })
    .RunConsoleAsync();
