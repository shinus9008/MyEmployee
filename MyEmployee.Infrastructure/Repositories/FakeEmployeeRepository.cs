using MyEmployee.Domain.AggregateModels.EmployeeAggregates;
using System.Collections.Concurrent;

namespace MyEmployee.Infrastructure.Repositories
{
    public class FakeEmployeeRepository : IEmployeeRepository
    {
        public  static Random random = new Random();
        public  static ConcurrentDictionary<int, EmployeeModel> data = new ConcurrentDictionary<int, EmployeeModel>();
        private static volatile int _counter;
        private static object _locker = new object();
        
        static FakeEmployeeRepository()
        {
            _counter = 10;
            for (int i = 0; i < _counter; i++) 
            {
                data.TryAdd(i, new EmployeeModel
                {
                    Id = i,
                    Birthday = DateTime.Now,
                    FirstName  = $"Ф: {i}",
                    LastName   = $"И: {i}",
                    MiddleName = $"О: {i}",
                    Sex = EmployeeSex.Male,
                    HaveChildren  = false,
                });
            }

           
        }

        public Task CreateAsync(EmployeeModel mode)
        {
            
            return Task.Run(() =>
            {
                lock (_locker) 
                {
                    data.TryAdd(_counter, mode);
                    _counter++;
                }
            });
        }

        public Task DeleteAsync(EmployeeModel model)
        {
            return Task.Run(() => data.Remove(model.Id, out EmployeeModel? value));
        }

        public Task UpdateAsync(EmployeeModel model)
        {
            return Task.Run(() => data.AddOrUpdate(model.Id, model, (i, m) => m));            
        }

        public async IAsyncEnumerable<EmployeeModel> GetAllAsync()
        {
            /* Перечислитель, возвращаемый из словаря, можно безопасно использовать одновременно с чтением и записью в словарь, 
             * однако он не представляет моментальный снимок словаря. 
             * Содержимое, предоставляемое через перечислитель, может содержать изменения, внесенные в словарь после вызова GetEnumerator.
             * https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.getenumerator?view=net-8.0&redirectedfrom=MSDN#System_Collections_Concurrent_ConcurrentDictionary_2_GetEnumerator
             */

            foreach (var item in data)
            {
                await Task.CompletedTask;
                yield return item.Value;
            }
        }

        public Task<EmployeeModel> GetRandom()
        {
            //TODO: Прверить, если удалили, то что ыернет!

            var count  = data.Count;
            var index  = random.Next(count);
            var result = data[index];
            return Task.FromResult(result);
        }

       
    }
}
