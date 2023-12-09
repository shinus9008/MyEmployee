using System.Collections.Generic;

namespace MyEmployee.Domain.SeedWork
{
    public interface IAsyncRepositoryReader<T> 
    {
        IAsyncEnumerable<T> GetAllAsync();
    }
}
