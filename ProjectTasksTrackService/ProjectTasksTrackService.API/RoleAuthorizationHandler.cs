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
        /// <summary>
        /// Метод определенияя успешности авторизации по
        /// https://learn.microsoft.com/ru-ru/dotnet/api/microsoft.aspnetcore.authorization.authorizationhandler-1.handlerequirementasync?view=aspnetcore-9.0#microsoft-aspnetcore-authorization-authorizationhandler-1-handlerequirementasync(microsoft-aspnetcore-authorization-authorizationhandlercontext-0)
        /// </summary>
        /// <param name="context"> AuthorizationHandlerContext-контекст авторизации</param>
        /// <param name="requirement"> OperationAuthorizationRequirement-требование для оценки успешности авторизации </param>
        /// <param name="resource"> ресурс </param>
        /// <returns></returns>
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
