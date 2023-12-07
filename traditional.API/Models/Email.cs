using System.ComponentModel.DataAnnotations;

namespace traditional.API.Models
{
    public class Email
    {
        public Email(string localPart, string domain)
        {
            LocalPart = localPart;
            Domain = domain;
        }

        [Key]
        public int? Id { get; set; }
        public string LocalPart { get; set; }
        public string Domain { get; set; }
    }
}
