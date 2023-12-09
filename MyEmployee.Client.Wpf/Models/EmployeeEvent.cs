namespace MyEmployee.Client.Wpf.Models
{
    public class EmployeeEvent
    {
        public Action Action { get; set; }
        public EmployeeModel Employee { get; set; }
    }
    public enum Action
    {
        New,
        Update,
        Delete
    }
}
