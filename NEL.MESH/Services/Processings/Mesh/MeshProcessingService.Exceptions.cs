// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
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
        }
    }
}
