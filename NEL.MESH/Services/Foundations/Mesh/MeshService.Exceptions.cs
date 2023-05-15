// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NEL.MESH.Models.Foundations.Chunking.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Foundations.Mesh
{
    internal partial class MeshService
    {
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<Message> RetruningMessageFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidArgumentsMeshException invalidArgumentMeshException)
            {
                throw CreateValidationException(invalidArgumentMeshException);
            }
            catch (HttpRequestException httpRequestException)
                when ((int)httpRequestException.StatusCode >= 400 && (int)httpRequestException.StatusCode <= 499)
            {
                var failedMeshClientException = new FailedMeshClientException(httpRequestException);

                throw CreateDependencyValidationException(failedMeshClientException);
            }
            catch (HttpRequestException httpRequestException)
                when ((int)httpRequestException.StatusCode >= 500 && (int)httpRequestException.StatusCode <= 599)
            {
                var failedMeshServerException = new FailedMeshServerException(httpRequestException);

                throw CreateDependencyException(failedMeshServerException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateServiceException(failedMeshServiceException);
            }
        }

        private async ValueTask<Message> TryCatch(RetruningMessageFunction retruningMessageFunction)
        {
            try
            {
                return await retruningMessageFunction();
            }
            catch (NullMessageException nullMessageException)
            {
                throw CreateValidationException(nullMessageException);
            }
            catch (NullHeadersException nullHeadersException)
            {
                throw CreateValidationException(nullHeadersException);
            }
            catch (InvalidArgumentsMeshException invalidArgumentMeshException)
            {
                throw CreateValidationException(invalidArgumentMeshException);
            }
            catch (InvalidMeshException invalidMeshException)
            {
                throw CreateValidationException(invalidMeshException);
            }
            catch (HttpRequestException httpRequestException)
                when ((int)httpRequestException.StatusCode >= 400 && (int)httpRequestException.StatusCode <= 499)
            {
                var failedMeshClientException = new FailedMeshClientException(httpRequestException);

                throw CreateDependencyValidationException(failedMeshClientException);
            }
            catch (HttpRequestException httpRequestException)
                when ((int)httpRequestException.StatusCode >= 500 && (int)httpRequestException.StatusCode <= 599)
            {
                var failedMeshServerException = new FailedMeshServerException(httpRequestException);

                throw CreateDependencyException(failedMeshServerException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateServiceException(failedMeshServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (HttpRequestException httpRequestException)
                when ((int)httpRequestException.StatusCode >= 400 && (int)httpRequestException.StatusCode <= 499)
            {
                var failedMeshClientException = new FailedMeshClientException(httpRequestException);

                throw CreateDependencyValidationException(failedMeshClientException);
            }
            catch (HttpRequestException httpRequestException)
                when ((int)httpRequestException.StatusCode >= 500 && (int)httpRequestException.StatusCode <= 599)
            {
                var failedMeshServerException = new FailedMeshServerException(httpRequestException);

                throw CreateDependencyException(failedMeshServerException);
            }
            catch (InvalidArgumentsMeshException invalidArgumentMeshException)
            {
                throw CreateValidationException(invalidArgumentMeshException);
            }
            catch (Exception exception)
            {
                var failedMeshServiceException =
                    new FailedMeshServiceException(exception);

                throw CreateServiceException(failedMeshServiceException);
            }
        }

        private MeshValidationException CreateValidationException(Xeption exception)
        {
            var meshValidationException = new MeshValidationException(exception);

            return meshValidationException;
        }

        private MeshDependencyValidationException CreateDependencyValidationException(Xeption exception)
        {
            var meshDependencyValidationException =
                new MeshDependencyValidationException(
                    exception.InnerException as Xeption);

            return meshDependencyValidationException;
        }

        private MeshDependencyException CreateDependencyException(Xeption exception)
        {
            var meshDependencyException =
                new MeshDependencyException(exception.InnerException as Xeption);

            throw meshDependencyException;
        }

        private MeshServiceException CreateServiceException(Xeption exception)
        {
            var meshServiceException = new
                MeshServiceException(exception);

            return meshServiceException;
        }
    }
}
