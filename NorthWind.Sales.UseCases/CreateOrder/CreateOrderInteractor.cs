namespace NorthWind.Sales.UseCases.CreateOrder
{
    public class CreateOrderInteractor : ICreateOrderInputPort
    {
        readonly ICreateOrderOutputPort OutputPort;
        readonly INorthWindSalesCommandsRepository Repository;

        public CreateOrderInteractor(ICreateOrderOutputPort outputPort,
            INorthWindSalesCommandsRepository repository)
        {
            OutputPort = outputPort;
            Repository = repository;
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
        }
    }
}
