// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.MESH.UI.Models
{
    public class Mailbox
    {
        public string Application { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MailboxId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string DestinationMailbox { get; set; } = string.Empty;
        public string WorkflowId { get; set; } = string.Empty;
        public string LocalId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool DefaultMailbox { get; set; } = false;
    }
}
