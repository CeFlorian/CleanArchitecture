namespace NorthWind.Sales.BusinessObjects.Interfaces.Repositories
{
    public interface IRefreshTokenCommandsRepository : IUnitOfWork
    {
        Task CreateRefreshToken(RefreshToken refreshToken);

    }
}
