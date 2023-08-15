using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.Presenters
{
    public class GetAllOrdersPresenter : IGetAllOrdersPresenter
    {
        public IEnumerable<Order> Orders { get; private set; }


        public Task Handle(IEnumerable<Order> orders)
        {
            Orders = orders;
            return Task.CompletedTask;
        }
    }
}
