using System.Threading.Tasks;

namespace MyEmployee.Domain.SeedWork
{
    public interface IRandomModelAccess<T>
    {
        Task<T> GetRandom();
    }
}
