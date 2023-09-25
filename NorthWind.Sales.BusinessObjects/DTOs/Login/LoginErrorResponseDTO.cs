namespace NorthWind.Sales.BusinessObjects.DTOs.Login
{
    public class LoginErrorResponseDTO : LoginResponse
    {
        public string Message { get; set; }
        public string Code { get; set; }
    }
}