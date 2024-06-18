namespace Auth.API.Models.Domain
{
    public class UserModel
    {
        public Guid Id { get; set; } 

        public string? Name { get; set; }    

        public string? UserName { get; set; }

        public string? Email { get; set; }   
    }
}
