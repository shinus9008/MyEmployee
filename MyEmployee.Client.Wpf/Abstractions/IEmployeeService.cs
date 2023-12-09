using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Services
{
    public interface IEmployeeService
    {
        Task CreateEmployee(EmployeeModel model);
        Task DeleteEmployee(EmployeeModel model);
        Task UpdateEmployee(EmployeeModel model);
        IAsyncEnumerable<EmployeeModel> GetAllEmployes(CancellationToken cancellationToken);
        IAsyncEnumerable<EmployeeEvent> GetAllEvents(CancellationToken cancellationToken);
    }


   
}