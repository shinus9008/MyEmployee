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
                                await foreach (var item in service.GetEvents(token))
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
        public static IObservable<IChangeSet<EmployeePoco, int>> GetObservableEmployee(
            this IEmployeeCache cache, 
            Func<CancellationToken,Task> backgroundTask,  
            IScheduler? scheduler = null)
        {
            //
            if (scheduler == null)
                scheduler = RxApp.TaskpoolScheduler;

            return Observable
                .Create<IChangeSet<EmployeePoco, int>>(observer =>
                {
                    var disposable = new SingleAssignmentDisposable();

                    try
                    {
                        // Если кто-то подписался: Запускаем фонувую задачу
                        disposable.Disposable = RxApp.TaskpoolScheduler.ScheduleAsync((sch, token) => backgroundTask(token));
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                    // Если нет подпичиков вызовиться Disposable
                    return new CompositeDisposable(
                        disposable,                                     // Завершаем фонувую задачу
                        Disposable.Create(observer.OnCompleted),        // Оповещаем подписчиков озавершении? //TODO: может и не надо надо тестить
                        cache.Source.Connect().SubscribeSafe(observer)  // Отписываемся от кеша 
                        );
                })
                .Publish()
                .RefCount(1);
        }
    }
}
