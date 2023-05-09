// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Clients.Token.Exceptions;
using NEL.MESH.Models.Foundations.Tokens.Exceptions;
using NEL.MESH.Models.Processings.Tokens;
using Xeptions;

namespace NEL.MESH.Services.Processings.Tokens
{
    internal partial class TokenProcessingService : ITokenProcessingService
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (TokenValidationException tokenValidationException)
            {
                throw CreateProcessingDependencyValidationException(tokenValidationException);
            }
            catch (TokenDependencyValidationException tokenDependencyValidationException)
            {
                throw CreateProcessingDependencyValidationException(tokenDependencyValidationException);
            }
            catch (TokenDependencyException tokenDependencyException)
            {
                throw CreateProcessingDependencyException(tokenDependencyException);
            }
            catch (TokenServiceException tokenServiceException)
            {
                throw CreateProcessingDependencyException(tokenServiceException);
            }
        }

        private static TokenProcessingDependencyValidationException CreateProcessingDependencyValidationException(
            Xeption exception)
        {
            return new TokenProcessingDependencyValidationException(exception.InnerException as Xeption);
        }

        private static TokenProcessingDependencyException CreateProcessingDependencyException(
            Xeption exception)
        {
            return new TokenProcessingDependencyException(exception.InnerException as Xeption);
        }
    }
}
