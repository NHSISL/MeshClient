// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using NEL.MESH.Models.Foundations.Mesh;
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

        private static void ValidateMeshMessageOnSendMessage(Message message)
        {
            ValidateMessageIsNotNull(message);
            ValidateHeadersIsNotNull(message);
            Validate(
                    (Rule: IsInvalid(message.From), Parameter: nameof(Message.From)),
                    (Rule: IsInvalid(message.To), Parameter: nameof(Message.To)),
                    (Rule: IsInvalid(message.WorkflowId), Parameter: nameof(Message.WorkflowId)),
                    (Rule: IsInvalid(message.Headers), Parameter: nameof(Message.Headers)),
                    (Rule: IsInvalid(message.Body), Parameter: nameof(Message.Body)),
                    (Rule: IsInvalid(message.Headers, "Content-Type"), Parameter: "Content-Type"),
                    (Rule: IsInvalid(message.Headers, "Mex-FileName"), Parameter: "Mex-FileName"),
                    (Rule: IsInvalid(message.Headers, "Mex-From"), Parameter: "Mex-From"),
                    (Rule: IsInvalid(message.Headers, "Mex-To"), Parameter: "Mex-To"),
                    (Rule: IsInvalid(message.Headers, "Mex-WorkflowID"), Parameter: "Mex-WorkflowID"));
        }

        private static void ValidateMessageIsNotNull(Message message)
        {
            if (message is null)
            {
                throw new NullMessageException();
            }
        }

        private static void ValidateHeadersIsNotNull(Message message)
        {
            if (message.Headers is null)
            {
                throw new NullHeadersException();
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Dictionary<string, List<string>> dictionary) => new
        {
            Condition = dictionary.Count == 0,
            Message = "Header values required"
        };

        private static dynamic IsInvalid(Dictionary<string, List<string>> dictionary, string key) => new
        {
            Condition = IsInvalidKey(dictionary, key),
            Message = "Header value is required"
        };

        private static bool IsInvalidKey(Dictionary<string, List<string>> dictionary, string key)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return false;
            }

            bool keyExists = dictionary.ContainsKey(key);

            if (!keyExists)
            {
                return true;
            }

            string value = dictionary[key].FirstOrDefault();

            return String.IsNullOrWhiteSpace(value);
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidMeshException = new InvalidMeshException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidMeshException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidMeshException.ThrowIfContainsErrors();
        }
    }
}
