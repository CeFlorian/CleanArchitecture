using NorthWind.Sales.BusinessObjects.Interfaces.EventBus.Bus;
using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.UseCases.CreateOrder
{
    public class CreateOrderInteractor : ICreateOrderInputPort
    {
        readonly ICreateOrderOutputPort OutputPort;
        readonly INorthWindSalesCommandsRepository Repository;
        readonly IEventBus EventBus;


        public CreateOrderInteractor(ICreateOrderOutputPort outputPort,
            INorthWindSalesCommandsRepository repository, IEventBus eventBus)
        {
            OutputPort = outputPort;
            Repository = repository;
            EventBus = eventBus;
        }

        public async ValueTask Handle(CreateOrderDTO orderDTO)
        {
            OrderAggregate orderAggregate = new OrderAggregate
            {
                CustomerId = orderDTO.CustomerId,
                ShipAddress = orderDTO.ShipAddress,
                ShipCity = orderDTO.ShipCity,
                ShipCountry = orderDTO.ShipCountry,
                ShipPostalCode = orderDTO.ShipPostalCode
            };

            foreach (var item in orderDTO.OrderDetails)
            {
                orderAggregate.AddDetail(item.ProductId, item.UnitPrice, item.Quantity);
            }

            await Repository.CreateOrder(orderAggregate);
            await Repository.SaveChanges();
            await OutputPort.Handle(orderAggregate.Id);

            EventBus.Publish(new OrderCreatedEvent()
            {
                CustomerId = orderDTO.CustomerId,
                ShipAddress = orderDTO.ShipAddress,
                ShipCity = orderDTO.ShipCity,
                ShipCountry = orderDTO.ShipCountry,
                ShipPostalCode = orderDTO.ShipPostalCode,
                OrderDetails = orderDTO.OrderDetails
            });

        }
    }
}
