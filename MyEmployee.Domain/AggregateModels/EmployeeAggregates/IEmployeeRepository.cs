using MyEmployee.Domain.SeedWork;
using System.Threading.Tasks;

namespace MyEmployee.Domain.AggregateModels.EmployeeAggregates
{
    public interface IEmployeeRepository :
        IAsyncRepositoryReader<EmployeeModel>,
        IRandomModelAccess<EmployeeModel>
    {
        Task UpdateAsync(EmployeeModel model);
    }
}
