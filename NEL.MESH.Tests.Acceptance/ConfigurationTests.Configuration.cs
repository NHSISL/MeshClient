// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using Xunit;

namespace NEL.MESH.Tests.Acceptance
{
    public partial class ConfigurationTests
    {
        [Fact]
        public void ShouldGetConfigurationSettings()
        {
            // given
            var mailboxId = this.configuration["MeshConfiguration:MailboxId"];
            var password = this.configuration["MeshConfiguration:Password"];
            //var mexClientVersion = this.configuration["MeshConfiguration:MexClientVersion"];
            //var mexOSName = this.configuration["MeshConfiguration:MexOSName"];
            //var mexOSVersion = this.configuration["MeshConfiguration:MexOSVersion"];
            //var key = this.configuration["MeshConfiguration:Key"];
            //var clientCert = this.configuration["MeshConfiguration:ClientCertificate"];
            //var rootCert = this.configuration["MeshConfiguration:RootCertificate"];

            //string[] intermediateCertificates =
            //    this.configuration.GetSection("MeshConfiguration:IntermediateCertificates").Get<string[]>();

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            //mexClientVersion.Should().NotBeNullOrEmpty();
            //mexOSName.Should().NotBeNullOrEmpty();
            //mexOSVersion.Should().NotBeNullOrEmpty();
            //key.Should().NotBeNullOrEmpty();
            //clientCert.Should().NotBeNullOrEmpty();
            //rootCert.Should().NotBeNullOrEmpty();

            mailboxId.Should().StartWith("QM");
            password.Should().StartWith("0q");
            //key.Should().StartWith("Ba");
            //clientCert.Should().StartWith("MIITjgIBAzCCEzgGCSqGSIb3D");
            //rootCert.Should().StartWith("LS0tLS1CRUdJTiBDRVJU");
        }

        [Fact]
        public void ShouldGetEnvironmentVariables()
        {
            // given
            var mailboxId = Environment.GetEnvironmentVariable("NEL_MESH_CLIENT_ACCEPTANCE_MeshConfiguration__MailboxId");
            var password = Environment.GetEnvironmentVariable("NEL_MESH_CLIENT_ACCEPTANCE_MeshConfiguration__Password");
            //var mexClientVersion = Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__MexClientVersion");
            //var mexOSName = Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__MexOSName");
            //var mexOSVersion = Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__MexOSVersion");
            //var key = Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__Key");
            //var clientCert = Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__ClientCertificate");
            //var rootCert = Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__RootCertificate");

            //string[] intermediateCertificates =
            //    Environment.GetEnvironmentVariable("NEL__MESH__CLIENT__ACCEPTANCE__MeshConfiguration__IntermediateCertificates").Split(',');

            // then
            mailboxId.Should().NotBeNullOrEmpty();
            password.Should().NotBeNullOrEmpty();
            //mexClientVersion.Should().NotBeNullOrEmpty();
            //mexOSName.Should().NotBeNullOrEmpty();
            //mexOSVersion.Should().NotBeNullOrEmpty();
            //key.Should().NotBeNullOrEmpty();
            //clientCert.Should().NotBeNullOrEmpty();
            //rootCert.Should().NotBeNullOrEmpty();

            mailboxId.Should().StartWith("QM");
            password.Should().StartWith("0q");
            //key.Should().StartWith("Ba");
            //clientCert.Should().StartWith("MIITjgIBAzCCEzgGCSqGSIb3D");
            //rootCert.Should().StartWith("LS0tLS1CRUdJTiBDRVJU");
        }
    }
}
