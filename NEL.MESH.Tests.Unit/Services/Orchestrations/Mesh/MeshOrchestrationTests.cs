// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Moq;
using NEL.MESH.Services.Foundations.Mesh;
using NEL.MESH.Services.Foundations.Tokens;
using NEL.MESH.Services.Orchestrations.Mesh;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Services.Orchestrations.Mesh
{
    public partial class MeshOrchestrationTests
    {
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly Mock<IMeshService> meshServiceMock;
        private readonly IMeshOrchestrationService meshOrchestrationService;

        public MeshOrchestrationTests()
        {
            this.tokenServiceMock = new Mock<ITokenService>();
            this.meshServiceMock = new Mock<IMeshService>();

            this.meshOrchestrationService = new MeshOrchestrationService(
                tokenService: this.tokenServiceMock.Object,
                meshService: this.meshServiceMock.Object);
        }

        private static string GetRandomString(int wordCount = 0)
        {
            return new MnemonicString(
                wordCount: wordCount == 0 ? GetRandomNumber() : wordCount,
                wordMinLength: 1,
                wordMaxLength: GetRandomNumber()).GetValue();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
