// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace NEL.MESH.Services.Processings.Tokens
{
    internal interface ITokenProcessingService
    {
        ValueTask<string> GenerateTokenAsync();
    }
}
