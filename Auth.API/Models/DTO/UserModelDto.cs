namespace Auth.API.Models.DTO
{
    public class UserModelDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }
    }
}
