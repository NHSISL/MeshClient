// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Tokens.Exceptions;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal partial class MeshOrchestrationService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Message> ReturningMessageFunction();
        private delegate ValueTask<List<string>> ReturningStringsFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (NullMeshMessageException nullMeshMessageException)
            {
                throw CreateValidationException(nullMeshMessageException);
            }
            catch (InvalidTokenException invalidTokenException)
            {
                throw CreateValidationException(invalidTokenException);
            }
            catch (InvalidMeshOrchestrationArgsException invalidMeshOrchestrationArgsException)
            {
                throw CreateValidationException(invalidMeshOrchestrationArgsException);
            }
            catch (TokenValidationException tokenValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenValidationException);
            }
            catch (TokenDependencyValidationException tokenDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (TokenDependencyException tokenDependencyException)
            {
                throw CreateDependencyException(tokenDependencyException);
            }
            catch (TokenServiceException tokenServiceException)
            {
                throw CreateDependencyException(tokenServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshOrchestrationServiceException =
                    new FailedMeshOrchestrationServiceException(
                        message: "Failed mesh orchestration service occurred, please contact support", 
                        innerException: exception);

                throw CreateServiceException(failedMeshOrchestrationServiceException);
            }
        }

        private async ValueTask<Message> TryCatch(ReturningMessageFunction returningMessageFunction)
        {
            try
            {
                return await returningMessageFunction();
            }
            catch (NullMeshMessageException nullMeshMessageException)
            {
                throw CreateValidationException(nullMeshMessageException);
            }
            catch (InvalidTokenException invalidTokenException)
            {
                throw CreateValidationException(invalidTokenException);
            }
            catch (InvalidMeshOrchestrationArgsException invalidMeshOrchestrationArgsException)
            {
                throw CreateValidationException(invalidMeshOrchestrationArgsException);
            }
            catch (TokenValidationException tokenValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenValidationException);
            }
            catch (TokenDependencyValidationException tokenDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (TokenDependencyException tokenDependencyException)
            {
                throw CreateDependencyException(tokenDependencyException);
            }
            catch (TokenServiceException tokenServiceException)
            {
                throw CreateDependencyException(tokenServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshOrchestrationServiceException = new FailedMeshOrchestrationServiceException(
                        message: "Failed mesh orchestration service occurred, please contact support",
                        innerException: exception);

                throw CreateServiceException(failedMeshOrchestrationServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringsFunction returningStringsFunction)
        {
            try
            {
                return await returningStringsFunction();
            }
            catch (InvalidTokenException invalidTokenException)
            {
                throw CreateValidationException(invalidTokenException);
            }
            catch (InvalidMeshOrchestrationArgsException invalidMeshOrchestrationArgsException)
            {
                throw CreateValidationException(invalidMeshOrchestrationArgsException);
            }
            catch (TokenValidationException tokenValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenValidationException);
            }
            catch (TokenDependencyValidationException tokenDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenDependencyValidationException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshDependencyValidationException);
            }
            catch (TokenDependencyException tokenDependencyException)
            {
                throw CreateDependencyException(tokenDependencyException);
            }
            catch (TokenServiceException tokenServiceException)
            {
                throw CreateDependencyException(tokenServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshOrchestrationServiceException = new FailedMeshOrchestrationServiceException(
                        message: "Failed mesh orchestration service occurred, please contact support",
                        innerException: exception);

                throw CreateServiceException(failedMeshOrchestrationServiceException);
            }
        }

        private MeshOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var meshOrchestrationDependencyValidationException = new MeshOrchestrationDependencyValidationException(
                    message: "Mesh orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            return meshOrchestrationDependencyValidationException;
        }

        private MeshOrchestrationValidationException CreateValidationException(Xeption exception)
        {
            var meshOrchestrationValidationException = new MeshOrchestrationValidationException(exception);

            return meshOrchestrationValidationException;
        }

        private MeshOrchestrationDependencyException CreateDependencyException(Xeption exception)
        {
            var meshOrchestrationDependencyException = new MeshOrchestrationDependencyException(
                    message: "Mesh orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            throw meshOrchestrationDependencyException;
        }

        private MeshOrchestrationServiceException CreateServiceException(Xeption exception)
        {
            var meshOrchestrationServiceException = new MeshOrchestrationServiceException(
                    message: "Mesh orchestration service error occurred, contact support.",
                    innerException: exception);

            throw meshOrchestrationServiceException;
        }
    }
}
