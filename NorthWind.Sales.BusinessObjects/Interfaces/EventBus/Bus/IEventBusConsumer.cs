using NorthWind.Sales.BusinessObjects.POCOEntities.Events;

namespace NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus
{
    public interface IEventBusConsumer : IDisposable
    {
        Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
        Task Unsubscribe<T, TH>() where T : Event where TH : IEventHandler<T>;

    }
}
