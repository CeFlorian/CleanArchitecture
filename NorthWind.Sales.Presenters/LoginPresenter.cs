using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.Presenters
{
    public class LoginPresenter : ILoginPresenter
    {
        public LoginResponse Response { get; private set; }

        public Task Handle(LoginResponse response)
        {
            Response = response;
            return Task.CompletedTask;
        }
    }
}
