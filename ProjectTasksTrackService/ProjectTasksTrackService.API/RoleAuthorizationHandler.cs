using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ProjectTasksTrackService.API.Contracts.Dto.Requests.Auth;

namespace ProjectTasksTrackService.API
{
    /// <summary>
    /// Класс для авторизации по ролям (в методах Delete...)
    /// </summary>
    public class RoleAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, LoginRequestDto>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            LoginRequestDto resource)
        {
            if (context.User == null)
                return Task.CompletedTask;

            if (context.User.IsInRole("PM") || context.User.IsInRole("admin"))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
