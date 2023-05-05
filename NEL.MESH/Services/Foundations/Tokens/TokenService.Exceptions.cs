// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Token.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal partial class TokenService : ITokenService
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
                throw CreateAndLogValidationException(invalidArgumentTokenException);
            }
            catch (Exception exception)
            {
                var failedTokenServiceException =
                    new FailedTokenServiceException(exception);

                throw CreateAndLogServiceException(failedTokenServiceException);
            }
        }

        private TokenValidationException CreateAndLogValidationException(Xeption exception)
        {
            var tokenValidationException =
                new TokenValidationException(exception);

            return tokenValidationException;
        }

        private TokenServiceException CreateAndLogServiceException(Xeption exception)
        {
            var tokenServiceException = new
                TokenServiceException(exception);

            return tokenServiceException;
        }
    }
}
