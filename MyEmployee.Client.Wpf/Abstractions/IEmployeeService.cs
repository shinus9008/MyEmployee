using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Services
{
    public interface IEmployeeService
    {
        IAsyncEnumerable<EmployeeModel> GetAllEmployes(CancellationToken cancellationToken);

        IAsyncEnumerable<EmployeeModel> GetAllEvents(CancellationToken cancellationToken);
    }


   
}