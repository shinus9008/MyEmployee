using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Services;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace MyEmployee.Client.Wpf.SmaleTasks
{
    public class SynchroSmallTask : ISmallTask
    {
        private readonly IEmployeeCache   cache;
        private readonly IEmployeeService service;

        public SynchroSmallTask(IEmployeeCache cache, IEmployeeService service)
        {
            this.cache = cache;
            this.service = service;
        }

        public Task DoAsync(CancellationToken token)
        {
            // Слушаем все сообщения 
            // Бкферизируем
            // Обновляем кеш
            return service
                .GetObservableEmployeeEvent()
                .Buffer(TimeSpan.FromMilliseconds(100), RxApp.TaskpoolScheduler)
                .Where(x => x.Count != 0)
                .Do(items =>
                {
                    //TODO: Забыл как ловить исключения в Щиукмфиду
                    try
                    {
                        cache.Source.Edit(edit =>
                        {
                            foreach (var item in items)
                            {
                                if (item != null)
                                {
                                    switch (item.Action)
                                    {
                                        //TODO:Улучшить!
                                        case Models.EmployeeEventAction.UpdateOrCreate:                                       
                                            edit.AddOrUpdate(item.Employee);
                                            break;
                                        case Models.EmployeeEventAction.Delete:
                                            edit.RemoveKey(item.Employee.Id);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        });
                    }
                    catch (Exception)
                    {                      
                    }
                })                
                .RunAsync(token)
                .ToTask();
        }
    }
}
