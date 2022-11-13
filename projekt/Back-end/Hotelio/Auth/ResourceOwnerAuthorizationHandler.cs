using System.Security.Claims;
using Hotelio.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Hotelio.Auth
{
    public class ResourceOwnerAuthorizationHandler : AuthorizationHandler<ResourceOwnerRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOwnerRequirement requirement, IUserOwnedResource resource)
        {
            if (context.User.IsInRole(HotelRoles.Admin) ||
                context.User.IsInRole(HotelRoles.Owner) ||
                context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) == resource.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public record ResourceOwnerRequirement : IAuthorizationRequirement;
}
