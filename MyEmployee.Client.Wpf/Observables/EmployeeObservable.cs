using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using MyEmployee.Client.Wpf.SmaleTasks;

namespace MyEmployee.Client.Wpf.Observables
{
    /// <summary>
    /// При первом запуске (подписке) запускает фоновую зачачу
    /// </summary>
    public class EmployeeObservable : IEmployeeObservable
    {
        private readonly LoadingSmaleTask loadingSmaleTask;
        private readonly SynchroSmaleTask synchroSmaleTask;
                
        /// <inheritdoc/>
        public IObservable<IChangeSet<EmployeePoco, int>> Connect { get; }

        public EmployeeObservable(
            IEmployeeCache employeeCache, 
            LoadingSmaleTask loadingSmaleTask, 
            SynchroSmaleTask synchroSmaleTask)
        {
            this.loadingSmaleTask = loadingSmaleTask;
            this.synchroSmaleTask = synchroSmaleTask;

            // При подписке создается фоновая задача            
            this.Connect = employeeCache
                .GetObservableEmployee(BackgroundWorker);
                
        }

        /// <summary>
        /// Фоновый поток 
        /// </summary>
        /// <remarks>
        /// Выполняет две задачи (последовательно)
        /// </remarks>
        private async Task BackgroundWorker(CancellationToken token)
        {
            // #1 Загружем всех сотрудников в кеш
            await loadingSmaleTask.DoAsync(token);

            // #2 Синхронизируем кеш
            await synchroSmaleTask.DoAsync(token);
        }
    }
}
