namespace Auth.API.Models.Domain
{
    public class Student
    {
        public Guid Id { get; set; }

        public string? StudentName { get; set; }

        public string? Class { get; set; }

        public decimal? RollNumber { get; set; }

        public string? Address { get; set; }
        public int MobileNumber { get; set; }
    }
}
