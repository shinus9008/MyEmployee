using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.Observables
{
    /// <summary>
    /// Линиво заролняет кеш
    /// </summary>
    public class FakeEmployeeCache_Static : IEmployeeCache
    {
        SourceCache<EmployeeModel, int> cache = new SourceCache<EmployeeModel, int>(l => l.Key);

        /// <inheritdoc/>      
        public IObservableCache<EmployeeModel, int> Cache => cache;


        public FakeEmployeeCache_Static()
        {
            InitializeTask();
        }

        async Task InitializeTask()
        {
            for (int i = 0; i < 1000; i++)
            {
                cache.AddOrUpdate(new EmployeeModel()
                {
                    Key = i,
                });

                // Ждем; Продолжаем уже не в контексе синхронизации окна
                await Task.Delay(100).ConfigureAwait(false);
            }
        }
    }
}
