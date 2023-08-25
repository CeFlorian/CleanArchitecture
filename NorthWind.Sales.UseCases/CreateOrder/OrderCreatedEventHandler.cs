using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.Interfaces.Repositories.Consumer;
using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.UseCases.CreateOrder
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {

        readonly INorthWindConsumerCommandsRepository Repository;

        public OrderCreatedEventHandler(INorthWindConsumerCommandsRepository repository)
        {
            Repository = repository;
        }
        public async Task<bool> Handle(OrderCreatedEvent @event)
        {
            OrderAggregate orderAggregate = new OrderAggregate
            {
                Id = @event.Id,
                CustomerId = @event.CustomerId,
                ShipAddress = @event.ShipAddress,
                ShipCity = @event.ShipCity,
                ShipCountry = @event.ShipCountry,
                ShipPostalCode = @event.ShipPostalCode
            };

            foreach (var item in @event.OrderDetails)
            {
                orderAggregate.AddDetail(item.ProductId, item.UnitPrice, item.Quantity);
            }

            await Repository.CreateOrder(orderAggregate);

            return true;

        }


    }
}