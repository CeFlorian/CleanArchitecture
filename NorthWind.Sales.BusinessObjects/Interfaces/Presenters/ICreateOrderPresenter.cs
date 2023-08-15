using NorthWind.Sales.BusinessObjects.Interfaces.Ports;

namespace NorthWind.Sales.BusinessObjects.Interfaces.Presenters
{
    public interface ICreateOrderPresenter : ICreateOrderOutputPort
    {
        int OrderId { get; }
    }
}
