namespace NorthWind.Sales.BusinessObjects.Interfaces.Services
{
    public interface ICryptographyService
    {
        string GenerateSalt();

        string HashPassword(string password, string salt);
    }
}
