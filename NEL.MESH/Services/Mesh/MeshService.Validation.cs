// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Foundations.Mesh.Exceptions;
using Xeptions;

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
            Validate<InvalidMeshException>(
                (Rule: IsInvalid(message.StringContent), Parameter: nameof(Message.StringContent)),
                (Rule: IsInvalid(message.Headers, "Content-Type"), Parameter: "Content-Type"),
                (Rule: IsInvalid(message.Headers, "Mex-FileName"), Parameter: "Mex-FileName"),
                (Rule: IsInvalid(message.Headers, "Mex-From"), Parameter: "Mex-From"),
                (Rule: IsInvalid(message.Headers, "Mex-To"), Parameter: "Mex-To"),
                (Rule: IsInvalid(message.Headers, "Mex-WorkflowID"), Parameter: "Mex-WorkflowID"));
        }

        private static void ValidateMeshMessageOnSendFile(Message message)
        {
            ValidateMessageIsNotNull(message);
        }

        public static void ValidateTrackMessageArguments(string messageId)
        {
            Validate<InvalidMeshArgsException>(
               (Rule: IsInvalid(messageId), Parameter: nameof(Message.MessageId)));
        }

        private static string GetKey(Dictionary<string, List<string>> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key].FirstOrDefault();
            }

            return null;
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

        private static dynamic IsNotSame(
                 string first,
                 string second,
                 string secondName) => new
                 {
                     Condition = !String.IsNullOrWhiteSpace(first) && first != second,
                     Message = $"Requires a matching header value for key '{secondName}'"
                 };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(Dictionary<string, List<string>> dictionary, string key) => new
        {
            Condition = IsInvalidKey(dictionary, key),
            Message = "Header value is required"
        };

        private static dynamic IsArgInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static bool IsInvalidKey(Dictionary<string, List<string>> dictionary, string key)
        {
            if (dictionary == null)
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

        private static void Validate<T>(params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T));

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidDataException.ThrowIfContainsErrors();
        }

        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }
    }
}
