namespace MyEmployee.Client.Wpf.Models
{
    public class EmployeePoco
    {
        //TODO: Сделать через конструктор или Read onlY?
        public int Id { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
    }
}
