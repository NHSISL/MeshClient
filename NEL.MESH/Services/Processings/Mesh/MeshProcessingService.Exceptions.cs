// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        private delegate ValueTask<List<string>> ReturningStringsMeshFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidArgumentsMeshProcessingException invalidArgumentsMeshProcessingException)
            {
                throw CreateProcessingValidationException(invalidArgumentsMeshProcessingException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateProcessingDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateProcessingDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateProcessingDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateProcessingDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                throw CreateProcessingServiceException(exception);
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
                throw CreateProcessingValidationException(invalidArgumentsMeshProcessingException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateProcessingDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateProcessingDependencyValidationException(meshDependencyValidationException);
            }
            catch (MeshDependencyException meshDependencyException)
            {
                throw CreateProcessingDependencyException(meshDependencyException);
            }
            catch (MeshServiceException meshServiceException)
            {
                throw CreateProcessingDependencyException(meshServiceException);
            }
            catch (Exception exception)
            {
                throw CreateProcessingServiceException(exception);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringsMeshFunction returningStringsMeshFunction)
        {
            try
            {
                return await returningStringsMeshFunction();
            }
            catch (InvalidArgumentsMeshProcessingException invalidArgumentsMeshProcessingException)
            {
                throw CreateProcessingValidationException(invalidArgumentsMeshProcessingException);
            }
            catch (MeshValidationException meshValidationException)
            {
                throw CreateProcessingDependencyValidationException(meshValidationException);
            }
            catch (MeshDependencyValidationException meshDependencyValidationException)
            {
                throw CreateProcessingDependencyValidationException(meshDependencyValidationException);
            }
        }

        private static MeshProcessingValidationException CreateProcessingValidationException(Xeption exception)
        {
            return new MeshProcessingValidationException(exception);
        }

        private static MeshProcessingDependencyValidationException CreateProcessingDependencyValidationException(
            Xeption exception)
        {
            return new MeshProcessingDependencyValidationException(exception.InnerException as Xeption);
        }

        private static MeshProcessingDependencyException CreateProcessingDependencyException(
            Xeption exception)
        {
            return new MeshProcessingDependencyException(exception.InnerException as Xeption);
        }

        private static MeshProcessingServiceException CreateProcessingServiceException(
            Exception exception)
        {
            var failedMeshProcessingServiceException =
                new FailedMeshProcessingServiceException(exception);

            return new MeshProcessingServiceException(failedMeshProcessingServiceException);
        }
    }
}
