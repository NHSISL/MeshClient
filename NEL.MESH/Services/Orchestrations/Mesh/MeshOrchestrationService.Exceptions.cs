// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using NEL.MESH.Models.Processing.Mesh.Exceptions;
using NEL.MESH.Models.Processings.Mesh.Exceptions;
using NEL.MESH.Models.Processings.Tokens.Exceptions;
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
            catch (NullMeshMessageProcessingException nullMeshProcessingMessageException)
            {
                throw CreateValidationException(nullMeshProcessingMessageException);
            }
            catch (Models.Orchestrations.Mesh.Exceptions.InvalidTokenException invalidTokenException)
            {
                throw CreateValidationException(invalidTokenException);
            }
            catch (InvalidMeshOrchestrationArgsException invalidMeshOrchestrationArgsException)
            {
                throw CreateValidationException(invalidMeshOrchestrationArgsException);
            }
            catch (InvalidMeshProcessingArgsException invalidMeshProcessingArgsException)
            {
                throw CreateValidationException(invalidMeshProcessingArgsException);
            }
            catch (TokenProcessingValidationException tokenProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenProcessingValidationException);
            }
            catch (TokenProcessingDependencyValidationException tokenProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (TokenProcessingDependencyException tokenProcessingDependencyException)
            {
                throw CreateDependencyException(tokenProcessingDependencyException);
            }
            catch (TokenProcessingServiceException tokenProcessingServiceException)
            {
                throw CreateDependencyException(tokenProcessingServiceException);
            }
            catch (MeshProcessingDependencyException meshProcessingDependencyException)
            {
                throw CreateDependencyException(meshProcessingDependencyException);
            }
            catch (MeshProcessingServiceException meshProcessingServiceException)
            {
                throw CreateDependencyException(meshProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(exception);

                throw CreateServiceException(failedMeshProcessingServiceException);
            }
        }

        private async ValueTask<Message> TryCatch(ReturningMessageFunction returningMessageFunction)
        {
            try
            {
                return await returningMessageFunction();
            }
            catch (NullMeshMessageProcessingException nullMeshMessageProcessingException)
            {
                throw CreateValidationException(nullMeshMessageProcessingException);
            }
            catch (Models.Orchestrations.Mesh.Exceptions.InvalidTokenException invalidTokenException)
            {
                throw CreateValidationException(invalidTokenException);
            }
            catch (InvalidMeshOrchestrationArgsException invalidMeshOrchestrationArgsException)
            {
                throw CreateValidationException(invalidMeshOrchestrationArgsException);
            }
            catch (InvalidArgumentsMeshProcessingException invalidArgumentsMeshProcessingException)
            {
                throw CreateValidationException(invalidArgumentsMeshProcessingException);
            }
            catch (TokenProcessingValidationException tokenProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenProcessingValidationException);
            }
            catch (TokenProcessingDependencyValidationException tokenProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (TokenProcessingDependencyException tokenProcessingDependencyException)
            {
                throw CreateDependencyException(tokenProcessingDependencyException);
            }
            catch (TokenProcessingServiceException tokenProcessingServiceException)
            {
                throw CreateDependencyException(tokenProcessingServiceException);
            }
            catch (MeshProcessingDependencyException meshProcessingDependencyException)
            {
                throw CreateDependencyException(meshProcessingDependencyException);
            }
            catch (MeshProcessingServiceException meshProcessingServiceException)
            {
                throw CreateDependencyException(meshProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(exception);

                throw CreateServiceException(failedMeshProcessingServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringsFunction returningStringsFunction)
        {
            try
            {
                return await returningStringsFunction();
            }
            catch (InvalidArgumentsMeshProcessingException invalidArgumentsMeshProcessingException)
            {
                throw CreateValidationException(invalidArgumentsMeshProcessingException);
            }
            catch (Models.Orchestrations.Mesh.Exceptions.InvalidTokenException invalidTokenException)
            {
                throw CreateValidationException(invalidTokenException);
            }
            catch (InvalidMeshOrchestrationArgsException invalidMeshOrchestrationArgsException)
            {
                throw CreateValidationException(invalidMeshOrchestrationArgsException);
            }
            catch (TokenProcessingValidationException tokenProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenProcessingValidationException);
            }
            catch (TokenProcessingDependencyValidationException tokenProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(tokenProcessingDependencyValidationException);
            }
            catch (MeshProcessingValidationException meshProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingValidationException);
            }
            catch (MeshProcessingDependencyValidationException meshProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(meshProcessingDependencyValidationException);
            }
            catch (TokenProcessingDependencyException tokenProcessingDependencyException)
            {
                throw CreateDependencyException(tokenProcessingDependencyException);
            }
            catch (TokenProcessingServiceException tokenProcessingServiceException)
            {
                throw CreateDependencyException(tokenProcessingServiceException);
            }
            catch (MeshProcessingDependencyException meshProcessingDependencyException)
            {
                throw CreateDependencyException(meshProcessingDependencyException);
            }
            catch (MeshProcessingServiceException meshProcessingServiceException)
            {
                throw CreateDependencyException(meshProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(exception);

                throw CreateServiceException(failedMeshProcessingServiceException);
            }
        }

        private MeshOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var meshOrchestrationDependencyValidationException =
                new MeshOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            return meshOrchestrationDependencyValidationException;
        }

        private MeshOrchestrationValidationException CreateValidationException(Xeption exception)
        {
            var meshOrchestrationValidationException = new MeshOrchestrationValidationException(exception);

            return meshOrchestrationValidationException;
        }

        private MeshOrchestrationDependencyException CreateDependencyException(Xeption exception)
        {
            var meshOrchestrationDependencyException =
                new MeshOrchestrationDependencyException(exception.InnerException as Xeption);

            throw meshOrchestrationDependencyException;
        }

        private MeshOrchestrationServiceException CreateServiceException(Xeption exception)
        {
            var meshOrchestrationServiceException =
                new MeshOrchestrationServiceException(exception);

            throw meshOrchestrationServiceException;
        }
    }
}
