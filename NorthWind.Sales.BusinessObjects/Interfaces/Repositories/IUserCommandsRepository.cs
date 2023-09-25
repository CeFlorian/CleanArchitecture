namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories
{
    public interface IUserCommandsRepository : IUnitOfWork
    {
        Task Login(User user);

    }
}
