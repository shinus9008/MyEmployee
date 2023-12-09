
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;
using MyEmployee.Shared;

namespace MyEmployee.API.Services
{
    public class EmploeeUpdaterHostedService : BackgroundService
    {
        private readonly IServiceProvider services;

        public EmploeeUpdaterHostedService(IServiceProvider services)
        {
            this.services = services;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            { 
                try 
                {
                    await DoWork(stoppingToken);
                    await Task.Delay(1000, stoppingToken); 
                } 
                catch 
                { 
                    //TODO: Add loger
                }
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = services.CreateScope())
            {
                var repository =
                    scope.ServiceProvider
                        .GetRequiredService<IEmployeeRepository>();

                while (!stoppingToken.IsCancellationRequested) 
                {

                    var model =  await repository.GetRandom();
                    if( model != null)
                    {
                        model.FirstName = RandomHelper.RandomString(10);

                        await repository.UpdateAsync(model);
                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
