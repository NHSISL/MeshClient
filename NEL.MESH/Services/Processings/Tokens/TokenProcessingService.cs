// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace NEL.MESH.Services.Processings.Tokens
{
    internal class TokenProcessingService : ITokenProcessingService
    {
        private readonly ITokenProcessingService tokenProcessingService;

        public TokenProcessingService(ITokenProcessingService tokenProcessingService)
        {
            this.tokenProcessingService = tokenProcessingService;
        }

        ValueTask<string> ITokenProcessingService.GenerateTokenAsync() =>
            throw new System.NotImplementedException();
    }
}
