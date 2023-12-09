
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;

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
                        model.FirstName = RandomString(10);

                        await repository.UpdateAsync(model);
                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
