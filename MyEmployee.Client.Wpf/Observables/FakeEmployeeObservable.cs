using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using ReactiveUI;
using System.Reactive.Concurrency;

namespace MyEmployee.Client.Wpf.Observables
{
    /// <summary>
    /// Линиво заполняет кеш
    /// </summary>
    public class FakeEmployeeObservable : IEmployeeObservable
    {
        /// <inheritdoc/>     
        public IObservableCache<EmployeeModel, int> ObservableCache { get; }

        public FakeEmployeeObservable()
        {
            this.ObservableCache = 
                CreateBackgroundTask().AsObservableCache();
        }

        private IObservable<IChangeSet<EmployeeModel, int>> CreateBackgroundTask()
        {
            return ObservableChangeSet.Create<EmployeeModel, int>(cache =>
            {
                return RxApp.TaskpoolScheduler.ScheduleAsync((sch, token) => BackgroundWorker(cache, token));
            },
            x => x.Id);
        }

        private async Task BackgroundWorker(
          ISourceCache<EmployeeModel, int> cache, CancellationToken token)
        {
            for (int i = 0; i < 1000; i++)
            {
                cache.AddOrUpdate(new EmployeeModel()
                {
                    Id = i,
                });

                // Ждем; Продолжаем уже не в контексе синхронизации окна
                await Task.Delay(100, token).ConfigureAwait(false);
            }
        }
    }
}
