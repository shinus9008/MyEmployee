using MyEmployee.API.Abstractions;
using MyEmployee.API.Models;

namespace MyEmployee.API.Handlers
{
    public class EmployeeEventHandler : IEventHandler<EmployeeEvent>
    {
        public Task Handle(EmployeeEvent eventData)
        {
            return Task.CompletedTask;
        }
    }
}
