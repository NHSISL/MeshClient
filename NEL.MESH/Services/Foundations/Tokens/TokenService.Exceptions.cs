// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        }

        private TokenValidationException CreateAndLogValidationException(Xeption exception)
        {
            string validationSummary = GetValidationSummary(exception.Data);

            var meshValidationException =
                new TokenValidationException(exception, validationSummary);

            return meshValidationException;
        }
    }
}
