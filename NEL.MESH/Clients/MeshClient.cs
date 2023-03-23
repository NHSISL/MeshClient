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
    public class MeshClient : IMeshClient
    {
        public MeshClient(MeshConfigurations meshConfigurations)
        {
            IHost host = RegisterServices(meshConfigurations);
            Mailbox = InitializeClient(host);
        }

        public IMailboxClient Mailbox { get; private set; }

        private static IMailboxClient InitializeClient(IHost host) =>
            host.Services.GetRequiredService<IMailboxClient>();

        private static IHost RegisterServices(MeshConfigurations meshConfigurations)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(configuration =>
            {
                configuration.AddSingleton(options => meshConfigurations);
                configuration.AddTransient<IMeshBroker, MeshBroker>();
                configuration.AddTransient<IMeshService, MeshService>();
                configuration.AddTransient<IMailboxClient, MailboxClient>();
            });

            IHost host = builder.Build();

            return host;
        }
    }
}
