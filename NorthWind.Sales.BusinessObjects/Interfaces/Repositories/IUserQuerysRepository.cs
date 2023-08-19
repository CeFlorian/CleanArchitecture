namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories
{
    public interface IUserQuerysRepository : IUnitOfWork
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUserId(Guid userId);

    }
}