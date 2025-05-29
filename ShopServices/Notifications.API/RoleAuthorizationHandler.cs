using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Notifications.API
{
    /// <summary>
    /// Класс для авторизации по ролям в методах, требующих авторизацию
    /// </summary>
    public class RoleAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        /// <summary> Метод определения успешности авторизации по
        /// https://learn.microsoft.com/ru-ru/dotnet/api/microsoft.aspnetcore.authorization.authorizationhandler-1.handlerequirementasync?view=aspnetcore-9.0#microsoft-aspnetcore-authorization-authorizationhandler-1-handlerequirementasync(microsoft-aspnetcore-authorization-authorizationhandlercontext-0)
        /// </summary>
        /// <param name="context"> AuthorizationHandlerContext-контекст авторизации</param>
        /// <param name="requirement"> OperationAuthorizationRequirement-требование для оценки успешности авторизации </param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement)
        {
            if (context.User == null)
                return Task.CompletedTask;

            if (context.User.IsInRole("manager") ||
                //context.User.IsInRole("developer") ||
                context.User.IsInRole("admin"))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
