using NorthWind.Entities.Interfaces;

namespace NorthWind.Sales.UseCases.GetAllOrders
{
    internal class GetAllOrdersInteractor : IGetAllOrdersInputPort
    {
        readonly IGetAllOrdersOutputPort OutputPort;
        readonly INorthWindSalesQuerysRepository Repository;
        readonly ILogWritableRepository LogRepository;

        public GetAllOrdersInteractor(IGetAllOrdersOutputPort outputPort,
            INorthWindSalesQuerysRepository repository, ILogWritableRepository logRepository)
        {
            OutputPort = outputPort;
            Repository = repository;
            LogRepository = logRepository;
        }


        public async Task Handle()
        {
            var orders = await Repository.GetAllOrders();
            await LogRepository.Add("Obtener ordenes de compra");
            await OutputPort.Handle(orders);
        }
    }
}
