using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.Controllers
{
    public class GetAllOrdersController : IGetAllOrdersController
    {
        readonly IGetAllOrdersInputPort InputPort;
        readonly IGetAllOrdersPresenter Presenter;
        public GetAllOrdersController(IGetAllOrdersInputPort inputPort,
            IGetAllOrdersPresenter presenter) =>
            (InputPort, Presenter) = (inputPort, presenter);

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            await InputPort.Handle();
            return Presenter.Orders;
        }

    }
}
