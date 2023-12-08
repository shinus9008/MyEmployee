using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf.Observables
{
    internal class EmployeeScroller : IEmployeeCache
    {
        /// <summary>
        /// Кэш
        /// </summary>
        public IObservableCache<EmployeeModel, int> Cache { get; }


        public EmployeeScroller(
            IObservable<IEmployeeProvider> latest,
            IObservable<ScrollRequest> scrollRequest)
        {
            Cache = new SourceCache<EmployeeModel, int>(l => l.Key);


            //
            latest
                .CombineLatest(scrollRequest, (provider, scroll) => new { provider, scroll }); // 
                



            


        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ScrollRequest
    {
        //TODO: Добавить поля запроса страниц

        public static ScrollRequest All { get; } = new();
    }
}
