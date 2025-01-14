namespace AppX.Client.Domain.Entities.Common
{
    public class EmailObject
    {
        public EmailObject(string? from, string recipient, string? cc, string subject, string? body, string? attachedFile, string requestorEndpoint)
        {
            From = from;
            Recipient = recipient;
            Cc = cc;
            Subject = subject;
            Body = body;
            AttachedFile = attachedFile;
            RequestorEndpoint = requestorEndpoint;
        }

        public string? From { get; set; }
        public string Recipient { get; set; }
        public string? Cc { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public string? AttachedFile { get; set; } //For Enhancement - data type: File or Base64, for now it will be string.
        public string RequestorEndpoint { get; set; }

        //public EmailObject(string[] from, string[] recipient, string[]? cc, string subject, string? body, string[] attachedFile, string requestorEndpoint)
        //{
        //    Recipient = recipient;
        //    Subject = subject;
        //    Body = body;
        //    From = from;
        //    Cc = cc;
        //    AttachedFile = attachedFile;
        //    RequestorEndpoint = requestorEndpoint;
        //}

        //public string[] From { get; set; }
        //public string[] Recipient { get; set; }
        //public string[]? Cc { get; set; }
        //public string Subject { get; set; }
        //public string? Body { get; set; }
        //public string[] AttachedFile { get; set; } //For Enhancement - data type: File or Base64, for now it will be string.
        //public string RequestorEndpoint { get; set; }
    }
}
