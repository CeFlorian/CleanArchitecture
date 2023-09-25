using NorthWind.Sales.BusinessObjects.Interfaces.Ports;

namespace NorthWind.Sales.BusinessObjects.Interfaces.Presenters
{
    public interface ILoginPresenter : ILoginOutputPort
    {
        LoginResponse Response { get; }
    }
}
