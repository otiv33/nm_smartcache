using smartcache.API.Models;

namespace smartcache.API
{
    public static class EmailHelper
    {
        public static Email ParseEmail(string email)
        {
            var split = email.Split("@");
            return new Email(split[0], split[1]);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
