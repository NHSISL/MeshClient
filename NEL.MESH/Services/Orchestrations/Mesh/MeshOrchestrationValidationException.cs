// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal class MeshOrchestrationValidationException : Xeption
    {
        private const string validationMessage = "Mesh orchestration validation errors occurred, please try again.";

        public MeshOrchestrationValidationException(Xeption innerException, string validationSummary = "")
            : base(
                  message: validationSummary.Length > 0
                    ? $"{validationMessage}  Validation errors: {validationSummary}"
                    : validationMessage,
                  innerException)
        { }
    }
}
