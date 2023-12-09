namespace MyEmployee.API.Models
{
    public class EmployeePoco
    {
        public int Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
    }
}
