// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Moq;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Services.Orchestrations.Mesh;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Clients.Mailboxes
{
    public partial class MailboxClientTests
    {
        private readonly Mock<IMeshOrchestrationService> meshOrchestrationServiceMock;
        private readonly IMailboxClient mailboxClient;

        public MailboxClientTests()
        {
            this.meshOrchestrationServiceMock = new Mock<IMeshOrchestrationService>();

            this.mailboxClient = new MailboxClient(
                meshOrchestrationService: this.meshOrchestrationServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 2, wordMaxLength: 20).GetValue();
    }
}
