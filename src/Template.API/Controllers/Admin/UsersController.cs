using Microsoft.AspNetCore.Mvc;
using Template.API.Constants;
using Template.Application.Features.Users.Commands.AddUser;

namespace Template.API.Controllers.Admin
{
    public class UsersController : AdminBaseController
    {

        [HttpPost()]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Succeeded)
                return CreatedAtRoute(routeName: RouteNames.Users.GetUserById, routeValues: new { id = result.Data!.Id }, value: result);
            return NewResult(result);
        }



        ///// <summary>
        ///// Update the authenticated user's profile information
        ///// </summary>
        ///// <param name="command">Updated profile data including address and phone number</param>
        ///// <returns>Success response if profile is updated</returns>
        ///// <response code="200">Profile updated successfully</response>
        ///// <response code="400">Invalid input data</response>
        ///// <response code="401">User not authenticated</response>
        ///// <response code="404">User not found</response>
        //[HttpPatch("profile:{OwnerUserId:int}")]
        //[Authorize]
        //[ProducesResponseType(typeof(Result<UpdateProfileCommandResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdateProfile([FromRoute] int OwnerUserId, [FromBody] UpdateProfileCommand command)
        //{
        //    command.OwnerUserId = OwnerUserId;
        //    var result = await Mediator.Send(command);
        //    return NewResult(result);
        //}

    }
}
