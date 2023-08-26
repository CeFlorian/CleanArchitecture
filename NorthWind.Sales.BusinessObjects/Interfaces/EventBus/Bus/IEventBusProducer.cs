using NorthWind.Sales.BusinessObjects.POCOEntities.Events;

namespace NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus
{
    public interface IEventBusProducer
    {
        Task Publish<T>(T @event) where T : Event;

    }
}