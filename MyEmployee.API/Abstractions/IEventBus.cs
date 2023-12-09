namespace MyEmployee.API.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent eventData);
    }
}
