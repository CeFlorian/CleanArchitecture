namespace NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
    {
        Task<bool> Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}