using Microsoft.AspNetCore.Authorization;

namespace NorthWind.Demo.API.Authorization
{
    public class GetWeatherHandler : AuthorizationHandler<GetWeatherRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GetWeatherRequirement requirement)
        {
            //Es posible cambiar por ir a la base de datos de este microservicio y obtener permisos con el ID del usuario
            if (context.User.HasClaim(c => c.Type == "scope" && c.Value.Contains("can_read_weather")))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
