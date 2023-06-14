namespace Talabat.Core.Entities.EmailSettings
{
    public class Email
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        //   IList<IFormFile>? attachments { get; set; }

    }
}
