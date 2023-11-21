// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Tokens.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal partial class TokenService
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidTokenArgsException invalidArgumentTokenException)
            {
                throw CreateValidationException(invalidArgumentTokenException);
            }
            catch (Exception exception)
            {
                var failedTokenServiceException =
                    new FailedTokenServiceException(innerException: exception, data: exception.Data);

                throw CreateServiceException(failedTokenServiceException);
            }
        }

        private TokenValidationException CreateValidationException(Xeption exception)
        {
            var tokenValidationException =
                new TokenValidationException(innerException: exception);

            return tokenValidationException;
        }

        private TokenServiceException CreateServiceException(Xeption exception)
        {
            var tokenServiceException = new
                TokenServiceException(innerException: exception);

            return tokenServiceException;
        }
    }
}
