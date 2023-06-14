namespace Talabat.Core.Entities.EmailSettings
{
    public class Email
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public bool IsHtml { get; set; } // Added property to indicate if the email body is HTML or plain text
        //   IList<IFormFile>? attachments { get; set; }

    }
}
