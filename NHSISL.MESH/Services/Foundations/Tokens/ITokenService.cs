// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace NHSISL.MESH.Services.Foundations.Tokens
{
    internal interface ITokenService
    {
        ValueTask<string> GenerateTokenAsync();
    }
}
