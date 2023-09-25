namespace NorthWind.Sales.BusinessObjects.Enums
{
    public enum ErrorCodes
    {
        AnUnexpectedErrorOcurred = 100,
        CredentialsAreNotValid = 101,
        AccessTokenIsNotValid = 102,
        RefreshTokenIsNotActive = 103,
        RefreshTokenHasExpired = 104,
        RefreshTokenIsNotCorrect = 105,
        UserDoesNotExist = 106
    }
}
