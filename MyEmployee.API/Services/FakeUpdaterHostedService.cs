
using MyEmployee.API.Models;
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;
using MyEmployee.Shared;
using System.Reactive.Subjects;

namespace MyEmployee.API.Services
{
    /// <summary>
    /// Фоновый поток который иметирует обновление базы 
    /// </summary>
    /// 
    /// <remarks>
    /// Кривая заглушка
    /// </remarks>
    public class FakeUpdaterHostedService : BackgroundService
    {
        private readonly IServiceProvider services;
        private readonly Subject<EmployeeEvent> subject = new Subject<EmployeeEvent>();
        public FakeUpdaterHostedService(IServiceProvider services)
        {
            this.services = services;            
        }

        public IObservable<EmployeeEvent> Observable => subject;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            { 
                try 
                {
                    await DoWork(stoppingToken);
                    await Task.Delay(1000, stoppingToken); 
                } 
                catch (Exception ex)
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
                    // Запрашиваем данные из репозитория 
                    // 
                    var model =  await repository.GetRandom();
                    if( model != null)
                    {
                        // Изменяемих
                        model.FirstName = RandomHelper.RandomString(10);

                        // Обновили базу данных
                        await repository.UpdateAsync(model);
                    }

                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
