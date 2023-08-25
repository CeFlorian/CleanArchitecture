namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer
{
    public interface INorthWindConsumerCommandsRepository
    {
        ValueTask CreateOrder(OrderAggregate order);
    }
}
