using NorthWind.Sales.BusinessObjects.DTOs.Login;

namespace NorthWind.Sales.BusinessObjects.Interfaces.Ports
{
    public interface ILoginInputPort
    {
        Task Handle(LoginRequestDTO request);
    }
}
