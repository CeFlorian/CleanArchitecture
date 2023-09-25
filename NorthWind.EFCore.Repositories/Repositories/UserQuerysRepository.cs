namespace NorthWind.EFCore.Repositories.Repositories
{
    public class UserQuerysRepository : IUserQuerysRepository
    {
        readonly NorthWindSalesContext Context;
        public UserQuerysRepository(
            NorthWindSalesContext context) =>
            Context = context;

        public Task<User> GetUserByEmail(string email)
        {
            return Task.FromResult(Context.Users.Where(u => u.Email == email).SingleOrDefault());
        }

        public async Task<User> GetUserByUserId(Guid userId)
        {
            return Context.Users.Where(u => u.Id == userId).SingleOrDefault();
        }

        public async ValueTask SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}
