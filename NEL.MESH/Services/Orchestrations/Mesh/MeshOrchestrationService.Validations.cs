﻿// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using NEL.MESH.Models.Foundations.Mesh;
using NEL.MESH.Models.Orchestrations.Mesh.Exceptions;
using Xeptions;

namespace NEL.MESH.Services.Orchestrations.Mesh
{
    internal partial class MeshOrchestrationService
    {
        private void ValidateToken(string token)
        {
            Validate<InvalidTokenException>(
                (Rule: IsInvalid(token), Parameter: "Token"));
        }

        private static void ValidateMessageIsNotNull(Message message)
        {
            if (message is null)
            {
                throw new NullMeshMessageException();
            }
        }

        private static void ValidateChunksOnSendMessage(List<Message> chunkedMessages)
        {
            Validate<InvalidMeshOrchestrationArgsException>(
                (Rule: IsInvalid(chunkedMessages), Parameter: "ChunkedMessages"));
        }

        private static void ValidateTrackMessageArgs(string messageId)
        {
            Validate<InvalidMeshOrchestrationArgsException>(
                (Rule: IsInvalid(messageId), Parameter: nameof(Message.MessageId)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(List<Message> chunkedMessages) => new
        {
            Condition = chunkedMessages is null || chunkedMessages.Count == 0,
            Message = "At least one chunk part required"
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
    }
}
