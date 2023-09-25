namespace NorthWind.EFCore.Repositories.Repositories
{
    internal class RefreshTokenCommandsRepository : IRefreshTokenCommandsRepository
    {
        readonly NorthWindSalesContext Context;
        public RefreshTokenCommandsRepository(
            NorthWindSalesContext context) =>
            Context = context;

        public async Task CreateRefreshToken(RefreshToken refreshToken)
        {
            await Context.AddAsync(refreshToken);
        }

        public async ValueTask SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}
