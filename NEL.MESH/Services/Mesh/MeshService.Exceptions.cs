// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Mesh
{
    internal partial class MeshService : IMeshService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (FailedMeshClientException failedMeshClientException)
            {
                throw CreateAndLogDependencyValidationException(failedMeshClientException);
            }
            catch (FailedMeshServerException failedMeshClientException)
            {
                throw CreateAndLogDependencyException(failedMeshClientException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateAndLogServiceException(failedMeshServiceException);
            }
        }

        private MeshDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var meshDependencyValidationException =
                new MeshDependencyValidationException(
                    exception.InnerException as Xeption);

            return meshDependencyValidationException;
        }

        private MeshDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var meshDependencyException =
                new MeshDependencyException(exception.InnerException as Xeption);

            throw meshDependencyException;
        }

        private MeshServiceException CreateAndLogServiceException(Xeption exception)
        {
            var meshServiceException = new
                MeshServiceException(exception);

            return meshServiceException;
        }
    }
}
