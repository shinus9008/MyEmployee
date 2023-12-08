using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Observables
{
    /// <summary>
    /// Кэш содрудников (реализация)
    /// </summary>
    public class EmployeeCache : IEmployeeCache
    {
        public ISourceCache<EmployeeModel, int> Source { get; } 
            = new SourceCache<EmployeeModel, int>(x => x.Key);
    }
}
