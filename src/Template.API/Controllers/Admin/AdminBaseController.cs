using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.Enums;

namespace Template.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
    public class AdminBaseController : BaseController
    {
    }
}
