# MESH CLIENT

<a href="https://digital.nhs.uk/services/message-exchange-for-social-care-and-health-meshm"><img src="https://raw.githubusercontent.com/NHSISL/MeshClient/main/Resources/MeshBannerNarrow.png"></a>
[![.NET](https://github.com/NHSISL/MeshClient/actions/workflows/dotnet.yml/badge.svg)](https://github.com/NHSISL/MeshClient/actions/workflows/dotnet.yml)
[![The Standard - COMPLIANT](https://img.shields.io/badge/The_Standard-COMPLIANT-2ea44f)](https://github.com/hassanhabib/The-Standard)
[![Nuget](https://img.shields.io/nuget/v/NEL.MESH?logo=nuget)](https://www.nuget.org/packages/NEL.MESH)
![Nuget](https://img.shields.io/nuget/dt/NEL.MESH?color=blue&label=Downloads)
<br/><br/>
# Introduction
Nel.Mesh is a Standard-Compliant .NET library built on top of (MESH) to enable software engineers to develop Mesh compliant solutions in .NET

**MESH** - the Message Exchange for Social Care and Health  providing the ability to share data directly between health and care organisations and is the nationally recognised mechanism for this method of data sharing.

[Link to NHS Digital](https://digital.nhs.uk/services/message-exchange-for-social-care-and-health-mesh)
<br/><br/>
# Standard-Compliance
This library was built according to The Standard. The library follows engineering principles, patterns and tooling as recommended by The Standard.

This library was also built to be a community lead effort in order to eventually have all mesh end points covered.
<br/><br/>

# Diagram of components

![](Resources/drawIo.png)

**NOTE**: Please find below the methods exposed on the Mesh Client.
<br/><br/>
# Install library?

You can get Nel.Mesh [Nuget](https://www.nuget.org/packages/NEL.MESH/) package by typing:
```powershell
Install-Package NEL.MESH
```

# Prerequisites

You'll need some things at different stages of your development to integrate with the MESH API. For each environment you use, you'll need a:
- MESH mailbox ID and password
- Transport Layer Security (TLS) certificate
- Shared secret to include in the MESH authorization header

To get started you will need to sign in or create a [developer account](https://onboarding.prod.api.platform.nhs.uk/).

In My applications and Teams Create a new application for the environments needed and from here after a max of 10 days you will be given the appropriate Active Api Keys.

## - Request a MESH mailbox
Once NHS Digital have approved your request to use the MESH API, you'll need to request a MESH Mailbox to use in a 'Path to Live integration environment'. This is how you'll interact with the MESH API. A MESH Mailbox is secure and only your organisation can access it.

To request a MESH Mailbox, you'll need to fill in an online form. It takes 5 to 10 minutes to complete.

You'll need to know:

- Your ODS code.
- The workflow groups or IDs for the files you plan to send or receive.
- The contact details of the person who will be managing the mailbox in your organisation.

[Apply Here For Mailbox](https://digital.nhs.uk/services/message-exchange-for-social-care-and-health-mesh/messaging-exchange-for-social-care-and-health-apply-for-a-mailbox)

**NOTE**: This could also take a few days to come pack with the details.

## - Prerequisites For Live **ONLY**
This is also called digital on-boarding. You'll need to submit information that demonstrates:
- You have a valid use case
- You can manage risks
- Your software conforms technically with the requirements for this API
- This API can only be used where there is a legal basis to do so and you will be asked you to demonstrate this as part of the digital onboarding process before your software goes live.
<br/><br/>

# Current Mesh Functionality

|   Method                      | Description   | Links to NHS Digital Mesh Documentation |
| --------                      | -----------   | --------------------------- |
| ValidateMailboxAccess         | Use this endpoint to check that MESH can be reached and that the authentication you are using is correct. This endpoint only needs to be called once every 24 hours. This endpoint updates the details of the connection history held for your mailbox and is similar to a keep-alive or ping message, in that it allows monitoring on the Spine to be aware of the active use of a mailbox despite a lack of traffic.| [Validate](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#post-/messageexchange/-mailbox_id-)|
| SendMessage                   | Use this endpoint to send a message via MESH. Use the POST command to your virtual outbox. Specify the message recipient in the request headers, with the message contained in the request body **Note**: If the file is too large the service will chunck this into smaller files.|[Send Message (String)](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#post-/messageexchange/-mailbox_id-/outbox)|
| SendFile                      | Use this endpoint to send a message via MESH. Use the POST command to your virtual outbox. Specify the message recipient in the request headers, with the message contained in the request body. **Note**: If the file is too large the service will chunck this into smaller files|[Send File (Byte)](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#post-/messageexchange/-mailbox_id-/outbox)|
| RetrieveTrackingStatusById    | Use this endpoint to inquire about the status of messages sent from your outbox. When determining the frequency of the calling of this endpoint consider that MESH is asynchronous, and it might be some hours until the recipient downloads your message. You must not poll this endpoint frequently.|   [Track By Id](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#get-/messageexchange/-mailbox_id-/outbox/tracking)                |
| RetrieveMessageIdsFromInbox   | Use this endpoint to retrieve a message based on the message identifier obtained from the 'Check Inbox' endpoint. Note: Headers should be treated case insensitively, most http clients will do this for you automatically, but please do not rely on explicit case. /messageexchange/{mailbox_id}/, if the file has been chunked the service will stitch back on retrieve. | [Get Messages](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#get-/messageexchange/-mailbox_id-/inbox/-message_id-)|
| RetrieveMessageById           | Use this endpoint to retrieve a message based on the message identifier obtained from the 'Check Inbox' endpoint. Note: Headers should be treated case insensitively, most http clients will do this for you automatically, but please do not rely on explicit case. /messageexchange/{mailbox_id}/inbox/{message_id},if the file has been chunked the service will stitch back on retrieve. | [Get Message By Id](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#get-/messageexchange/-mailbox_id-/inbox/-message_id-)|
| AcknowledgeMessageById        | Use this endpoint to acknowledge the successful download of a message.This operation: <ul><li>Closes the message transaction on Spine.</li><li>Removes the message from your mailbox inbox, which means that the message_id does not appear in subsequent calls to the 'Check inbox' endpoint and cannot be downloaded again Note: If you fail to acknowledge a message after five days in the inbox this sends a non-delivery report to the sender's inbox.</li></ul> | [Acknowledge](https://digital.nhs.uk/developer/api-catalogue/message-exchange-for-social-care-and-health-api#put-/messageexchange/-mailbox_id-/inbox/-message_id-/status/acknowledged)

# Local App Settings
To run this package you will need to setup a local app settings file with the following configuration items.  Certificates will need to be base64 encoded and the Intermediate can have multiples.

NOTE: The key value will have to be requested from NHS Digital as the version on their website was out of date at time of documenting

```
{
  "MeshConfiguration": 
  {
    "MailboxId": "",
    "Password": "",
    "Key": "",
    "Url": "",
    "MexClientVersion": "ApiDocs==0.0.1",
    "MexOSName": "Windows",
    "MexOSVersion": "#11",
    "RootCertificate": "",
    "IntermediateCertificates": [""],
    "ClientCertificate": "",
    "ClientCertificatePassword": "",
    "MaxChunkSizeInMegabytes": ""
  }
}

```

# How to Contribute
If you want to contribute to this project please before hand review the following documents to gain an understanding of the patterns and practices used in building this package:
- [The Standard](https://github.com/hassanhabib/The-Standard)
- [C# Coding Standard](https://github.com/hassanhabib/CSharpCodingStandard)
- [The Team Standard](https://github.com/hassanhabib/The-Standard-Team)

If you have a question make sure you open an issue.