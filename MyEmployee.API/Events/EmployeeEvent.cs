using MyEmployee.API.Abstractions;

namespace MyEmployee.API.Models
{
    public class EmployeeEvent : IntegrationEvent
    {
        public required EmployeeEventType Action { get; init; }
        public required EmployeePoco Employee { get; init; }
       
    }
    public enum EmployeeEventType
    {
        Create,
        Update,
        Delete,      
    }
}
