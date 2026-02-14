using Microsoft.AspNetCore.Authorization;
namespace Template.API.Authorization.Requirements;

public class SameUserOrAdminRequirement : IAuthorizationRequirement { }