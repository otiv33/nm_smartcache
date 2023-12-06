namespace smartcache.API.Models
{
    public class Email
    {
        public Email(string localPart, string domain)
        {
            LocalPart = localPart;
            Domain = domain;
        }

        public string LocalPart { get; set; }
        public string Domain { get; set; }
    }
}
