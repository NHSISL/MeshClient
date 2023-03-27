// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Moq;
using NEL.MESH.Brokers.DateTimes;
using NEL.MESH.Brokers.Identifiers;
using NEL.MESH.Services.Foundations.Tokens;
using Tynamix.ObjectFiller;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Tokens
{
    public partial class TokenServiceTests
    {
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly ITokenService tokenService;

        public TokenServiceTests()
        {
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();

            this.tokenService = new TokenService(
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object);
        }

        private static string GetRandomString(int wordCount = 0)
        {
            return new MnemonicString(
                wordCount: wordCount == 0 ? GetRandomNumber() : wordCount,
                wordMinLength: 1,
                wordMaxLength: GetRandomNumber()).GetValue();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private string HashStringSha256(string value, string key)
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
