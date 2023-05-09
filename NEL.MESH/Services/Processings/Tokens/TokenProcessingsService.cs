// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Identifiers;
using NEL.MESH.Brokers.Mesh;
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

        public async ValueTask<string> GenerateTokenAsync() =>
            await this.tokenService.GenerateTokenAsync();
    }
}
