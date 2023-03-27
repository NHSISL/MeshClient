// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal interface ITokenService
    {
        ValueTask<string> GenerateTokenAsync();
    }
}
