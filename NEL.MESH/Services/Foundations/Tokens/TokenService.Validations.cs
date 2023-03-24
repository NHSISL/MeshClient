// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NEL.MESH.Models.Foundations.Token.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal partial class TokenService : ITokenService
    {
        public static void ValidateGenerateTokenArgs(string mailboxId, string password, string key)
        {
            Validate<InvalidTokenArgsException>(
               (Rule: IsInvalid(mailboxId), Parameter: "MailboxId"),
               (Rule: IsInvalid(password), Parameter: "Password"),
               (Rule: IsInvalid(key), Parameter: "Key"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

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
