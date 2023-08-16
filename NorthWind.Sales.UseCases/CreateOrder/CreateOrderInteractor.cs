using NorthWind.Entities.Interfaces;

namespace NorthWind.Sales.UseCases.CreateOrder
{
    public class CreateOrderInteractor : ICreateOrderInputPort
    {
        readonly ICreateOrderOutputPort OutputPort;
        readonly INorthWindSalesCommandsRepository Repository;
        readonly IApplicationStatusLogger Logger;
        readonly ILogWritableRepository LogRepository;

        public CreateOrderInteractor(ICreateOrderOutputPort outputPort,
            INorthWindSalesCommandsRepository repository, IApplicationStatusLogger logger,
            ILogWritableRepository logRepository)
        {
            OutputPort = outputPort;
            Repository = repository;
            LogRepository = logRepository;
            Logger = logger;
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
            await LogRepository.Add("Crear orden de compra");
            await Repository.SaveChanges();
            await Logger.Log($"Orden de compra creada");
            await OutputPort.Handle(orderAggregate.Id);
        }
    }
}
