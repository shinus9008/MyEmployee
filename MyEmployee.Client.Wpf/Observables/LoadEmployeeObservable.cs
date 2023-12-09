using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using MyEmployee.Client.Wpf.SmaleTasks;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf.Observables
{
    /// <summary>
    /// При первом запуске (подписке) запускает зачачу <see cref="LoadingSmaleTask"/>
    /// </summary>
    public class LoadEmployeeObservable : IEmployeeObservable
    {
        private readonly IEmployeeCache employeeCache;       
        private readonly LoadingSmaleTask loadingSmaleTask;
        private readonly SynchroSmaleTask synchroSmaleTask;

        /// <inheritdoc/>      
        public IObservableCache<EmployeeModel, int> ObservableCache { get; }

        /// <inheritdoc/>  
        public LoadEmployeeObservable(IEmployeeCache employeeCache, LoadingSmaleTask loadingSmaleTask, SynchroSmaleTask synchroSmaleTask)
        {
            this.employeeCache    = employeeCache;          
            this.loadingSmaleTask = loadingSmaleTask;
            this.synchroSmaleTask = synchroSmaleTask;

            // При подписке создается фоновая задача
            // Поидее надо бы какойто планировщик задач, но нет времени)
            this.ObservableCache = CreateBackgroundTask()
                .AsObservableCache();
        }

        private IObservable<IChangeSet<EmployeeModel, int>> CreateBackgroundTask()
        {
            return Observable.Create<IChangeSet<EmployeeModel, int>>(observer =>
            {
               var cache = employeeCache.Source;
               var disposable = new SingleAssignmentDisposable();

               try
               {
                   disposable.Disposable = RxApp.TaskpoolScheduler.ScheduleAsync((sch, token) => BackgroundWorker(cache, token));
               }
               catch (Exception e)
               {
                   observer.OnError(e);
               }

               return new CompositeDisposable(disposable, Disposable.Create(observer.OnCompleted), cache.Connect().SubscribeSafe(observer), cache);
            });            
        }

        /// <summary>
        /// Фоновый поток 
        /// </summary>
        /// <remarks>
        /// Выполняет две задачи (последовательно)
        /// </remarks>
        private async Task BackgroundWorker(
            ISourceCache<EmployeeModel, int> cache, CancellationToken token)
        {
            // #1 Загружем всех сотрудников в кеш
            await loadingSmaleTask.DoAsync(token);

            // #2 Синхронизируем кеш
            await synchroSmaleTask.DoAsync(token);
        }
    }
}
