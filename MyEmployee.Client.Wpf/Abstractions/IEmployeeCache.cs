using DynamicData;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Abstractions
{
    /// <summary>
    /// 
    /// </summary>   
    public interface IEmployeeCache
    {
        IObservableCache<EmployeeModel, int> Cache { get; }

    }
}
