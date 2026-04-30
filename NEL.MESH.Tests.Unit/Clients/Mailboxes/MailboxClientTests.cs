// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Moq;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using NEL.MESH.Services.Orchestrations.Mesh;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

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

        public static TheoryData<Xeption> OrchestrationValidationExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new MeshOrchestrationValidationException(
                    message: "Mesh orchestration validation errors occurred, please try again.",
                    innerException),

                new MeshOrchestrationDependencyValidationException(
                    message: "Mesh orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> OrchestrationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new MeshOrchestrationDependencyException(
                    message: "Mesh orchestration dependency error occurred, fix the errors and try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> OrchestrationServiceExceptions()
        {
            string randomMessage = GetRandomString();
            var innerException = new Xeption(randomMessage);

            return new TheoryData<Xeption>
            {
                new MeshOrchestrationServiceException(
                    message: "Mesh orchestration service error occurred, contact support.",
                    innerException),
            };
        }
    }
}
