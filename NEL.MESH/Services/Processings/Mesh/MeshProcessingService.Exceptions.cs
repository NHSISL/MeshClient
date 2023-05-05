// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Processings.Mesh;
using Xeptions;

namespace NEL.MESH.Services.Processings.Mesh
{
    internal partial class MeshProcessingService : IMeshProcessingService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Message> ReturningMessageFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidArgumentsMeshProcessingException invalidArgumentsMeshProcessingException)
            {
                var meshProcessingValidationException =
                    new MeshProcessingValidationException(invalidArgumentsMeshProcessingException);

                throw meshProcessingValidationException;
            }
            catch (MeshValidationException meshValidationException)
            {
                var meshProcessingDependencyValidationException =
                    new MeshProcessingDependencyValidationException(
                        meshValidationException.InnerException as Xeption);

                throw meshProcessingDependencyValidationException;
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                var meshProcessingDependencyValidationException =
                    new MeshProcessingDependencyValidationException(
                        meshDependencyValidationException.InnerException as Xeption);

                throw meshProcessingDependencyValidationException;
            }
            catch (MeshDependencyException meshDependencyException)
            {
                var meshProcessingDependencyException =
                    new MeshProcessingDependencyException(
                        meshDependencyException.InnerException as Xeption);

                throw meshProcessingDependencyException;
            }
            catch (MeshServiceException meshServiceException)
            {
                var meshProcessingDependencyException =
                    new MeshProcessingDependencyException(
                        meshServiceException.InnerException as Xeption);

                throw meshProcessingDependencyException;
            }
            catch (Exception exception)
            {
                var failedMeshProcessingServiceException =
                    new FailedMeshProcessingServiceException(exception);

                var meshProcessingServiceException = new
                    MeshProcessingServiceException(failedMeshProcessingServiceException);

                throw meshProcessingServiceException;
            }
        }

        private async ValueTask<Message> TryCatch(ReturningMessageFunction returningMessageFunction)
        {
            try
            {
                return await returningMessageFunction();
            }
            catch (InvalidArgumentsMeshProcessingException invalidArgumentsMeshProcessingException)
            {
                var meshProcessingValidationException =
                    new MeshProcessingValidationException(invalidArgumentsMeshProcessingException);

                throw meshProcessingValidationException;
            }
        }
    }
}
