using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PopcornApi.Security.PolicyServices
{
    public class PolicyFilter(string policies, IAuthorizationService authorizationService) : IAsyncAuthorizationFilter
    {
        private readonly string[] policies = !string.IsNullOrEmpty(policies) ? policies.Split(";") : [];
        private readonly IAuthorizationService authorizationService = authorizationService;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (policies.Length == 0)
                return;

            foreach (var policy in policies)
            {
                var authorized = await authorizationService.AuthorizeAsync(context.HttpContext.User, policy.Trim());
                if (authorized.Succeeded)
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
            return;
        }
    }
}
