// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Moq;
using NEL.MESH.Services.Foundations.Tokens;
using NEL.MESH.Services.Processings.Tokens;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Services.Processings.Token
{
    public partial class TokenProcessingServiceTests
    {
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly ITokenProcessingService tokenProcessingService;

        public TokenProcessingServiceTests()
        {
            tokenServiceMock = new Mock<ITokenService>();
            tokenProcessingService = new TokenProcessingService(tokenServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static List<string> GetRandomStringList()
        {
            return Enumerable.Range(2, GetRandomNumber())
               .Select(item => GetRandomString())
               .ToList();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
