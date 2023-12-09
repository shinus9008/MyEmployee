using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Services
{
    public interface IEmployeeService
    {
        Task CreateEmployee(EmployeePoco model);
        Task DeleteEmployee(EmployeePoco model);
        Task UpdateEmployee(EmployeePoco model);
        IAsyncEnumerable<EmployeePoco> GetAllEmployes(CancellationToken cancellationToken);
        IAsyncEnumerable<EmployeeEvent> GetAllEvents(CancellationToken cancellationToken);
    }


   
}