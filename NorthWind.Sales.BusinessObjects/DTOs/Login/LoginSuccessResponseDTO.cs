namespace NorthWind.Sales.BusinessObjects.DTOs.Login
{
    public class LoginSuccessResponseDTO : LoginResponse
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
