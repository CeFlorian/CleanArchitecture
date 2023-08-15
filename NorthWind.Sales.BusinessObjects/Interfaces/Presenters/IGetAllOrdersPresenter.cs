using NorthWind.Sales.BusinessObjects.Interfaces.Ports;

namespace NorthWind.Sales.BusinessObjects.Interfaces.Presenters
{
    public interface IGetAllOrdersPresenter : IGetAllOrdersOutputPort
    {
        IEnumerable<Order> Orders { get; }
    }
}
