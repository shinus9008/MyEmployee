using DynamicData;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmployeeObservable
    {
        IObservableCache<EmployeeModel, int> ObservableCache { get; }
    }
}
