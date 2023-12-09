using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Services;

namespace MyEmployee.Client.Wpf.SmaleTasks
{
    /// <summary>
    /// Загружает с сервера всех работников в кэш
    /// </summary>
    public class LoadingSmallTask : ISmallTask
    {
        private readonly IEmployeeCache cache;
        private readonly IEmployeeService service;

        public LoadingSmallTask(
            IEmployeeCache cache, IEmployeeService service)
        {
            this.cache = cache;
            this.service = service;
        }

        public async Task DoAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var isComplite = await TryCompliteWork(token).ConfigureAwait(false);
                if (isComplite)
                    return;

                await Task.Delay(1000);
            }
        }
        public async Task<bool> TryCompliteWork(CancellationToken token)
        {
            try
            {
                await foreach (var item in service.GetAllEmployes(new CancellationToken()))
                {
                    if(item != null)
                    {
                        cache.Source.AddOrUpdate(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                
                //TODO: Пишем лог
            }

            return false;
        }
    }
}
