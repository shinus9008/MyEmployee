using DynamicData;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Abstractions
{
    /// <summary>
    /// Кэш содрудников
    /// </summary>   
    public interface IEmployeeCache
    {
        ISourceCache<EmployeePoco, int> Source { get; }
    }
}
