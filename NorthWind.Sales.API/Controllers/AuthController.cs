using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Sales.BusinessObjects.DTOs.Login;
using NorthWind.Sales.BusinessObjects.Interfaces.Controllers.Login;
using NorthWind.Sales.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly ILoginController LoginController;

        public AuthController(ILoginController loginController)
        {
            LoginController = loginController;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<LoginResponse> Login(LoginRequestDTO request)
        {
            return await LoginController.Login(request);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("RefreshToken")]
        //public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequestDTO request)
        //{
        //    return await RefreshTokenController.Execute(request);
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("SignOut")]
        //public async Task<SignOutResponse> SignOut(SignOutRequestDTO request)
        //{
        //    return await SignOutController.Execute(request);
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("CreateUser")]
        //public async Task<CreateUserResponse> CreateUser(CreateUserRequestDTO request)
        //{
        //    return await CreateUserController.Execute(request);
        //}

    }
}
