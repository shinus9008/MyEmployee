using DynamicData;
using MyEmployee.Client.Wpf.ViewModels;

namespace MyEmployee.Client.Wpf.Services
{
    internal class FakeEmployeeService
    {
        SourceCache<EmployeeModel, int> cache = new DynamicData.SourceCache<EmployeeModel, int>(i => i.Id);

        public FakeEmployeeService()
        {
            InitializerTask();
        }

        async Task InitializerTask()
        {
            for (int i = 0; i < 10; i++)
            {
                cache.AddOrUpdate(new EmployeeModel()
                {
                    Id = i,
                });
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        public IObservable<IChangeSet<EmployeeModel, int>> Connect => cache.Connect();
    }
}
