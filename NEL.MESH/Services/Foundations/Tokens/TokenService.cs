// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Identifiers;
using NEL.MESH.Models.Configurations;

namespace NEL.MESH.Services.Foundations.Tokens
{
    internal partial class TokenService : ITokenService
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly MeshConfiguration meshConfiguration;

        public TokenService(
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            MeshConfiguration meshConfiguration)
        {
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.meshConfiguration = meshConfiguration;
        }

        public ValueTask<string> GenerateTokenAsync() =>
            TryCatch(async () =>
            {
                ValidateGenerateTokenArgs(
                    this.meshConfiguration.MailboxId,
                    this.meshConfiguration.Password,
                    this.meshConfiguration.Key);

                string nonce = this.identifierBroker.GetIdentifier().ToString();
                string timeStamp = this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmm");
                string nonce_count = "0";

                string stringToHash =
                    $"{this.meshConfiguration.MailboxId}" +
                    $":{nonce}" +
                    $":{nonce_count}" +
                    $":{this.meshConfiguration.Password}" +
                    $":{timeStamp}";

                string sharedKey = GenerateSha256(stringToHash, this.meshConfiguration.Key);

                string token = await ValueTask
                    .FromResult($"NHSMESH {this.meshConfiguration.MailboxId}:{nonce}:{nonce_count}:{timeStamp}:{sharedKey}");

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
