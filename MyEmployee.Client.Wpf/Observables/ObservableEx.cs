using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using MyEmployee.Client.Wpf.Services;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MyEmployee
{
    public static class ObservableEx
    {
        /// <summary>
        /// При первой подписке запускается задача прослушивания событий <see cref="EmployeeEvent"/>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static IObservable<EmployeeEvent> GetObservableEmployeeEvent(this IEmployeeService service, IScheduler? scheduler = null)
        {
            //
            if (scheduler == null)
                scheduler = RxApp.TaskpoolScheduler;

            //
            return Observable
                .Create<EmployeeEvent>(observer =>
                {
                    // Запускем задачу
                    return scheduler.ScheduleAsync(async (sch, token) =>
                    {
                        // Пока не вызвана команда отмены задачи.
                        while (!token.IsCancellationRequested)
                        {
                            try
                            {
                                // Передаем все изменения подписчику
                                await foreach (var item in service.GetAllEvents(new CancellationToken()))
                                {
                                    observer.OnNext(item);                                    
                                }
                            }
                            catch
                            {
                                await Task.Delay(1000);
                            }
                        }

                        observer.OnCompleted();
                    });
                })
                .Publish()
                .RefCount(1);
        }

        /// <summary>
        /// При первой подписке запускается задача
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="backgroundTask"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static IObservable<IChangeSet<EmployeeModel, int>> GetObservableEmployee(
            this IEmployeeCache cache, 
            Func<CancellationToken,Task> backgroundTask,  
            IScheduler? scheduler = null)
        {
            //
            if (scheduler == null)
                scheduler = RxApp.TaskpoolScheduler;

            return Observable
                .Create<IChangeSet<EmployeeModel, int>>(observer =>
                {
                    var disposable = new SingleAssignmentDisposable();

                    try
                    {
                        disposable.Disposable = RxApp.TaskpoolScheduler.ScheduleAsync((sch, token) => backgroundTask(token));
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                    return new CompositeDisposable(disposable, Disposable.Create(observer.OnCompleted), cache.Source.Connect().SubscribeSafe(observer));
                })
                .Publish()
                .RefCount(1);
        }
    }
}
