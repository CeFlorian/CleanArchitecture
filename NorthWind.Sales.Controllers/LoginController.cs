using NorthWind.Sales.BusinessObjects.DTOs.Login;
using NorthWind.Sales.BusinessObjects.Interfaces.Controllers.Login;
using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.Controllers
{
    public class LoginController : ILoginController
    {
        readonly ILoginInputPort InputPort;
        readonly ILoginPresenter Presenter;
        public LoginController(ILoginInputPort inputPort,
            ILoginPresenter presenter) =>
            (InputPort, Presenter) = (inputPort, presenter);

        public async Task<LoginResponse> Login(LoginRequestDTO user)
        {
            await InputPort.Handle(user);
            return Presenter.Response;
        }

    }
}
