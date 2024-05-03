// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NHSISL.MESH.Models.Foundations.Tokens.Exceptions;
using NHSISL.MESH.Models.Foundations.Tokens.Exceptions;
using Xeptions;

namespace NHSISL.MESH.Services.Foundations.Tokens
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
                var failedTokenServiceException = new FailedTokenServiceException(
                        message: "Token service error occurred, contact support.",
                        innerException: exception, 
                        data: exception.Data);

                throw CreateServiceException(failedTokenServiceException);
            }
        }

        private TokenValidationException CreateValidationException(Xeption exception)
        {
            var tokenValidationException = new TokenValidationException(
                message: "Token validation errors occurred, please try again.",
                innerException: exception);

            return tokenValidationException;
        }

        private TokenServiceException CreateServiceException(Xeption exception)
        {
            var tokenServiceException = new TokenServiceException(
                message: "Token service error occurred, contact support.",
                innerException: exception);

            return tokenServiceException;
        }
    }
}
