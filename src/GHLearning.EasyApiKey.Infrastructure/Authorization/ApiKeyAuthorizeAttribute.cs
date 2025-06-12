using Microsoft.AspNetCore.Authorization;

namespace GHLearning.EasyApiKey.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class ApiKeyAuthorizeAttribute(string policyName) : AuthorizeAttribute(policyName)
{
}
