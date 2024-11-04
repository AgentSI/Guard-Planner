namespace Domain.Entities
{
    public class SmtpSettings
    {
        public string From { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RequiresAuthentication { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
