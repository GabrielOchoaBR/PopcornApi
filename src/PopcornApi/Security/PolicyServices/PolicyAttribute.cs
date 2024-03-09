using Microsoft.AspNetCore.Mvc;

namespace PopcornApi.Security.PolicyServices
{
    public class PolicyAttribute : TypeFilterAttribute
    {
        public PolicyAttribute(string Policies) : base(typeof(PolicyFilter))
        {
            Arguments = [Policies];
        }
    }
}
