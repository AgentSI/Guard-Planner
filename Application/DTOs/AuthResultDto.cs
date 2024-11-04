namespace Application.DTOs
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
