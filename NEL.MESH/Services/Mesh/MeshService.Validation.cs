﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Net.Http;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;

namespace NEL.MESH.Services.Mesh
{
    internal partial class MeshService : IMeshService
    {
        private static void ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
            {
                string message = $"{(int)response.StatusCode} - {response.ReasonPhrase}";
                var httpRequestException = new HttpRequestException(message);

                switch ((int)response.StatusCode)
                {
                    case var code when code >= 400 && code <= 499:
                        {
                            throw new FailedMeshClientException(httpRequestException);
                        }
                    case var code when code >= 500 && code <= 599:
                        {
                            throw new FailedMeshServerException(httpRequestException);
                        }
                    default:
                        {
                            throw new Exception(message);
                        }
                }
            }
        }
    }
}