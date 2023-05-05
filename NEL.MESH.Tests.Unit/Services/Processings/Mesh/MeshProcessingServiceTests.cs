// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Moq;
using NEL.MESH.Services.Foundations.Mesh;
using NEL.MESH.Services.Processings.Mesh;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        private readonly Mock<IMeshService> meshServiceMock;
        private readonly IMeshProcessingService meshProcessingService;

        public MeshProcessingServiceTests()
        {
            meshServiceMock = new Mock<IMeshService>();
            meshProcessingService = new MeshProcessingService(meshServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
