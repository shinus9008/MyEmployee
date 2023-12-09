namespace MyEmployee.Client.Wpf.Models
{
    public class EmployeeEvent
    {
        public EmployeeEventAction Action { get; }
        public EmployeePoco Employee { get; }
        public EmployeeEvent(EmployeePoco employee, EmployeeEventAction action)
        {
            Employee = employee;
            Action = action;
        }
    }
    public enum EmployeeEventAction
    {
        UpdateOrCreate,
        Delete
    }


}
