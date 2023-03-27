// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Token.Exceptions;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal partial class MeshOrchestrationService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Message> ReturningMessageFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
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
                throw CreateAndLogDependencyException(tokenDependencyException);
            }
            catch (TokenServiceException tokenServiceException)
            {
                throw CreateAndLogDependencyException(tokenServiceException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateAndLogDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateAndLogDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                var failedMeshOrchestrationServiceException =
                    new FailedMeshOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedMeshOrchestrationServiceException);
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
                throw CreateAndLogValidationException(nullMeshMessageException);
            }
        }

        private MeshOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var meshOrchestrationDependencyValidationException =
                new MeshOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            return meshOrchestrationDependencyValidationException;
        }

        private MeshOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var meshOrchestrationDependencyException =
                new MeshOrchestrationDependencyException(exception.InnerException as Xeption);

            throw meshOrchestrationDependencyException;
        }

        private MeshOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var meshOrchestrationServiceException =
                new MeshOrchestrationServiceException(exception);

            throw meshOrchestrationServiceException;
        }

        private MeshOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            string validationSummary = GetValidationSummary(exception.Data);

            var meshOrchestrationValidationException =
                new MeshOrchestrationValidationException(exception, validationSummary);

            return meshOrchestrationValidationException;
        }
    }
}
