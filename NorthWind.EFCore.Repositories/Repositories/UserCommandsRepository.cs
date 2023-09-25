namespace NorthWind.EFCore.Repositories.Repositories
{
    internal class UserCommandsRepository : IUserCommandsRepository
    {
        readonly NorthWindSalesContext Context;
        public UserCommandsRepository(
            NorthWindSalesContext context) =>
            Context = context;

        public async Task Login(User user)
        {
            await Context.AddAsync(user);

            foreach (var Item in user.RefreshTokens)
            {
                await Context.AddAsync(new RefreshToken
                {
                    Value = Item.Value,
                    ExpirationDate = Item.ExpirationDate
                });
            }
        }

        public async ValueTask SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}
