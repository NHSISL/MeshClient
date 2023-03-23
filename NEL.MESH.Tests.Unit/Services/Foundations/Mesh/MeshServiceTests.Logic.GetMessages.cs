// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh.ExternalModeld;
using Newtonsoft.Json;
using Xunit;

namespace NEL.MESH.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldGetMessagesAsync()
        {
            // given
            List<string> randomStrings = GetRandomStrings();
            GetMessagesResponse getMessagesResponse = new GetMessagesResponse { Messages = randomStrings };
            string jsonContent = JsonConvert.SerializeObject(getMessagesResponse, Formatting.None);
            List<string> expectedList = randomStrings;

            HttpResponseMessage responseMessage = CreateGetMessagesHttpResponseMessage(jsonContent);

            this.meshBrokerMock.Setup(broker =>
                broker.GetMessagesAsync())
                    .ReturnsAsync(responseMessage);

            List<string> expectedMessage = GetMessagesFromHttpResponseMessage(responseMessage);

            // when
            var actualList = await this.meshService.RetrieveMessagesAsync();

            // then
            actualList.Should().BeEquivalentTo(expectedList);

            this.meshBrokerMock.Verify(broker =>
                broker.GetMessagesAsync(),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
