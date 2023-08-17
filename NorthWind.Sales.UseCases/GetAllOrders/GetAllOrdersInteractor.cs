using Microsoft.Extensions.Logging;
using NorthWind.Entities.Interfaces;

namespace NorthWind.Sales.UseCases.GetAllOrders
{
    internal class GetAllOrdersInteractor : IGetAllOrdersInputPort
    {
        readonly IGetAllOrdersOutputPort OutputPort;
        readonly INorthWindSalesQuerysRepository Repository;
        readonly IApplicationStatusLogger Logger;
        readonly ILogWritableRepository LogRepository;
        readonly ILogger _logger;


        public GetAllOrdersInteractor(IGetAllOrdersOutputPort outputPort,
            INorthWindSalesQuerysRepository repository, IApplicationStatusLogger logger,
            ILogWritableRepository logRepository, ILogger<GetAllOrdersInteractor> log)
        {
            OutputPort = outputPort;
            Repository = repository;
            LogRepository = logRepository;
            Logger = logger;
            _logger = log;
        }


        public async Task Handle()
        {
            var orders = await Repository.GetAllOrders();
            await LogRepository.Add("Obtener ordenes de compra");
            _logger.LogInformation("[Custom] Obtener ordenes de compra");
            await OutputPort.Handle(orders);
        }
    }
}
