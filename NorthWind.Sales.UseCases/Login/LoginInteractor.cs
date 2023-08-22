using NorthWind.Sales.BusinessObjects.DTOs.Login;
using NorthWind.Sales.BusinessObjects.Enums;
using NorthWind.Sales.BusinessObjects.Interfaces.Services;
using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.UseCases.Login
{
    public class LoginInteractor : ILoginInputPort
    {
        readonly ILoginOutputPort OutputPort;
        readonly IAuthTokenService AuthTokenService;
        readonly IUserQuerysRepository UserQuerysRepository;
        readonly IRefreshTokenCommandsRepository RefreshTokenCommandsRepository;
        readonly ICryptographyService CryptographyService;

        readonly IWeatherAPIService WeatherAPIService;

        public LoginInteractor(ILoginOutputPort outputPort, IAuthTokenService authTokenService,
            IUserQuerysRepository serQuerysRepository, ICryptographyService cryptographyService,
            IRefreshTokenCommandsRepository refreshTokenCommandsRepository,
            IWeatherAPIService weatherAPIService)
        {
            OutputPort = outputPort;
            AuthTokenService = authTokenService;
            UserQuerysRepository = serQuerysRepository;
            RefreshTokenCommandsRepository = refreshTokenCommandsRepository;
            CryptographyService = cryptographyService;

            WeatherAPIService = weatherAPIService;
        }

        public async Task Handle(LoginRequestDTO request)
        {
            try
            {
                var user = await UserQuerysRepository.GetUserByEmail(request.Email);
                if (user == null)
                {
                    var response = new LoginErrorResponseDTO
                    {
                        Message = Enum.GetName(ErrorCodes.UserDoesNotExist),
                        Code = ErrorCodes.UserDoesNotExist.ToString("D")
                    };

                    await OutputPort.Handle(response);
                    return;
                }

                if (AreCredentialsValid(request.Password, user))
                {
                    var refreshToken = new RefreshToken
                    {
                        Value = await AuthTokenService.GenerateRefreshToken(),
                        Active = true,
                        ExpirationDate = DateTime.UtcNow.AddMinutes(await AuthTokenService.GetRefreshTokenLifetimeInMinutes()),
                        User = user
                    };

                    var idToken = await AuthTokenService.GenerateIdToken(user);
                    var accessToken = await AuthTokenService.GenerateAccessToken(user);

                    var response = new LoginSuccessResponseDTO
                    {
                        IdToken = idToken,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken.Value
                    };

                    await RefreshTokenCommandsRepository.CreateRefreshToken(refreshToken);
                    await RefreshTokenCommandsRepository.SaveChanges();
                    await OutputPort.Handle(response);

                    var result = await WeatherAPIService.GetWeatherForecasts(accessToken);

                    return;
                }
                else
                {
                    var response = new LoginErrorResponseDTO
                    {
                        Message = Enum.GetName(ErrorCodes.CredentialsAreNotValid),
                        Code = ErrorCodes.CredentialsAreNotValid.ToString("D")
                    };

                    await OutputPort.Handle(response);
                    return;
                }
            }
            catch (Exception ex)
            {
                var response = new LoginErrorResponseDTO
                {
                    Message = Enum.GetName(ErrorCodes.AnUnexpectedErrorOcurred),
                    Code = ErrorCodes.AnUnexpectedErrorOcurred.ToString("D")
                };

                await OutputPort.Handle(response);
                return;
            }
        }

        private bool AreCredentialsValid(string testPassword, User user)
        {
            var hash = CryptographyService.HashPassword(testPassword, user.Salt);
            return hash == user.Password;
        }

    }
}
