using NorthWind.Sales.BusinessObjects.POCOEntities.Events;

namespace NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus
{
    public interface IEventBus
    {
        //Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}