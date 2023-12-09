using MyEmployee.API.Models;

namespace MyEmployee.Shared
{
    public interface IEmployeeEventObservable
    {
        IObservable<EmployeeEvent> Observable { get; }
    }
}
