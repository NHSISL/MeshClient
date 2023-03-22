// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NEL.MESH.Brokers.Mesh;
using NEL.MESH.Clients.MeshClients;
using NEL.MESH.Models.Configurations;
using NEL.MESH.Services.Mesh;

namespace NEL.MESH.Clients
{
    public class Mesh : IMesh
    {
        public Mesh(MeshConfigurations meshConfigurations)
        {
            IHost host = RegisterServices(meshConfigurations);
            MeshClient = InitializeClient(host);
        }

        public IMeshClient MeshClient { get; private set; }

        private static IMeshClient InitializeClient(IHost host) =>
            host.Services.GetRequiredService<IMeshClient>();

        private static IHost RegisterServices(MeshConfigurations meshConfigurations)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(configuration =>
            {
                configuration.AddTransient<IMeshBroker, MeshBroker>();
                configuration.AddTransient<IMeshService, MeshService>();
                configuration.AddTransient<IMeshClient, MeshClient>();
                configuration.AddSingleton(options => meshConfigurations);
            });

            IHost host = builder.Build();

            return host;
        }
    }
}
