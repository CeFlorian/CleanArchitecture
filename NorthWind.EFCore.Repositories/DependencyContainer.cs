namespace NorthWind.EFCore.Repositories
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddEFRepositories(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName)
        {
            services.AddDbContext<NorthWindSalesContext>(options =>
            options.UseSqlServer(configuration
            .GetConnectionString(connectionStringName)));

            services.AddScoped<INorthWindSalesCommandsRepository,
                NorthWindSalesCommandsRepository>();
            services.AddScoped<INorthWindSalesQuerysRepository,
                NorthWindSalesQuerysRepository>();
            services.AddScoped<IUserCommandsRepository,
                UserCommandsRepository>();
            services.AddScoped<IUserQuerysRepository,
                UserQuerysRepository>();
            services.AddScoped<IRefreshTokenCommandsRepository,
                RefreshTokenCommandsRepository>();

            return services;
        }
    }
}
