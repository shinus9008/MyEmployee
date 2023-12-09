namespace MyEmployee.API.Abstractions
{
    public interface IEventHandler 
    {
        Task Handle(IntegrationEvent @event);
    }

    public interface IEventHandler<in TIntegrationEvent> : IEventHandler where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent eventData);

        Task IEventHandler.Handle(IntegrationEvent eventData)
        {
            return Handle((TIntegrationEvent)eventData);
        }
    }
}
