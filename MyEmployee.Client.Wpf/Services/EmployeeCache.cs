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
        public ISourceCache<EmployeePoco, int> Source { get; } 
            = new SourceCache<EmployeePoco, int>(x => x.Id);
    }
}
