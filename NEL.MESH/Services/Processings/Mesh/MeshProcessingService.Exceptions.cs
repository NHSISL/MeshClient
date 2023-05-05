// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using NEL.MESH.Models.Processings.Mesh;

namespace NEL.MESH.Services.Processings.Mesh
{
    internal partial class MeshProcessingService : IMeshProcessingService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();

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
                    new MeshProcessingDependencyValidationException(meshValidationException);

                throw meshProcessingDependencyValidationException;
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                var meshProcessingDependencyValidationException =
                    new MeshProcessingDependencyValidationException(meshDependencyValidationException);

                throw meshProcessingDependencyValidationException;
            }
            catch (MeshDependencyException meshDependencyException)
            {
                var meshProcessingDependencyException =
                    new MeshProcessingDependencyException(meshDependencyException);

                throw meshProcessingDependencyException;
            }
            catch (MeshServiceException meshServiceException)
            {
                var meshProcessingDependencyException =
                    new MeshProcessingDependencyException(meshServiceException);

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
    }
}
