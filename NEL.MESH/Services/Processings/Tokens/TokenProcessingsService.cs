// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

using NEL.MESH.Services.Foundations.Tokens;

namespace NEL.MESH.Services.Processings.Tokens
{
    internal partial class TokenProcessingService : ITokenProcessingService
    {
        private readonly ITokenService tokenService;

        public TokenProcessingService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public ValueTask<string> GenerateTokenAsync() =>
             TryCatch(async () =>
             {
                 string token = await this.tokenService.GenerateTokenAsync();

                 return token;
             });
    }
}
