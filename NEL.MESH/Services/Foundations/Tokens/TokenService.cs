// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Identifiers;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal class TokenService : ITokenService
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public TokenService(IDateTimeBroker dateTimeBroker, IIdentifierBroker identifierBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<string> GenerateTokenAsync(string mailboxId, string password, string key) =>
            throw new NotImplementedException();

    }
}
