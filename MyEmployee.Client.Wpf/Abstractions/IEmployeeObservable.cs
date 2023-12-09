using DynamicData;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Abstractions
{
    /// <summary>
    /// Кеш только для чтения (используется vm для привязки)
    /// </summary>
    public interface IEmployeeObservable
    {
        IObservable<IChangeSet<EmployeeModel, int>> Connect { get; }        
    }
}
