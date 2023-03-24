// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Identifiers;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal partial class TokenService : ITokenService
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public TokenService(IDateTimeBroker dateTimeBroker, IIdentifierBroker identifierBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<string> GenerateTokenAsync(string mailboxId, string password, string key) =>
            TryCatch(async () =>
            {
                ValidateGenerateTokenArgs(mailboxId, password, key);
                string nonce = this.identifierBroker.GetIdentifier().ToString();
                string timeStamp = this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmm");
                string nonce_count = "0";
                string stringToHash = $"{mailboxId}:{nonce}:{nonce_count}:{password}:{timeStamp}";
                string sharedKey = GenerateSha256(stringToHash, key);

                string token =
                    await ValueTask.FromResult($"NHSMESH {mailboxId}:{nonce}:{nonce_count}:{timeStamp}:{sharedKey}");

                return token;
            });

        private string GenerateSha256(string value, string key)
        {
            var crypt = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(value));

            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }

            return hash;
        }
    }
}
