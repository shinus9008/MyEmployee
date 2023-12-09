using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Observables
{
    /// <summary>
    /// Линиво заполняет кеш
    /// </summary>
    public class FakeEmployeeObservable : IEmployeeObservable
    {
        private readonly IEmployeeCache employeeCache;

        /// <inheritdoc/>
        public IObservable<IChangeSet<EmployeePoco, int>> Connect { get; }

        public FakeEmployeeObservable(IEmployeeCache employeeCache)
        {
            this.Connect = employeeCache
                .GetObservableEmployee(BackgroundWorker);
            this.employeeCache = employeeCache;
        }

        private async Task BackgroundWorker(CancellationToken token)
        {
            for (int i = 0; i < 1000; i++)
            {
                employeeCache.Source.AddOrUpdate(new EmployeePoco()
                {
                    Id = i,
                });

                // Ждем; Продолжаем уже не в контексе синхронизации окна
                await Task.Delay(100, token).ConfigureAwait(false);
            }
        }
    }
}
