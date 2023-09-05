namespace NorthWind.Sales.UseCases.GetAllOrders
{
    public class GetAllOrdersInteractor : IGetAllOrdersInputPort
    {
        readonly IGetAllOrdersOutputPort OutputPort;
        readonly INorthWindSalesQuerysRepository Repository;

        public GetAllOrdersInteractor(IGetAllOrdersOutputPort outputPort,
            INorthWindSalesQuerysRepository repository)
        {
            OutputPort = outputPort;
            Repository = repository;
        }


        public async Task Handle()
        {
            var orders = await Repository.GetAllOrders();
            await OutputPort.Handle(orders);
        }
    }
}
