namespace Auth.API.Models.DTO
{
    public class TokenResponseDto
    {
        public string? TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
