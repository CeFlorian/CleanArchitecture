namespace NorthWind.Sales.BusinessObjects.Interfaces.Ports
{
    public interface ILoginOutputPort
    {
        Task Handle(LoginResponse response);
    }
}
