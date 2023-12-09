using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Services;

namespace MyEmployee.Client.Wpf.SmaleTasks
{
    public class SynchroSmaleTask : ISmaleTask
    {
        private readonly IEmployeeCache   cache;
        private readonly IEmployeeService service;

        public SynchroSmaleTask(IEmployeeCache cache, IEmployeeService service)
        {
            this.cache = cache;
            this.service = service;
        }

        public async Task DoAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await InternalAsync(token);
                }
                catch (Exception ex)
                {
                    //TODO: Add logger
                }
            }
        }

        private async Task InternalAsync(CancellationToken token)
        {
            
        }
    }
}
