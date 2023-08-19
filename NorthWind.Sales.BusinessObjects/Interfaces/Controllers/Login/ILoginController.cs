using NorthWind.Sales.BusinessObjects.DTOs.Login;

namespace NorthWind.Sales.BusinessObjects.Interfaces.Controllers.Login
{
    public interface ILoginController
    {
        Task<LoginResponse> Login(LoginRequestDTO request);

    }
}
