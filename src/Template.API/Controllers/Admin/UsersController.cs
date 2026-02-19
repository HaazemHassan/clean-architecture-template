using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.API.Common.Constants;
using Template.API.Requests.Management.Users;
using Template.Application.Features.Users.Commands.AddUser;
using Template.Application.Features.Users.Commands.UpdateProfile;

namespace Template.API.Controllers.Admin
{
    public class UsersController(IMapper _mapper) : AdminBaseController
    {

        [HttpPost()]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return CreatedAtRoute(routeName: RouteNames.Users.GetUserById, routeValues: new { id = result.Value!.Id }, value: result.Value);
        }



        /// <summary>
        /// Update the authenticated user's profile information
        /// </summary>
        /// <param name="command">Updated profile data including address and phone number</param>
        /// <returns>Success response if profile is updated</returns>
        /// <response code="200">Profile updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="401">User not authenticated</response>
        /// <response code="404">User not found</response>
        [HttpPatch("{userId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(UpdateProfileCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserRequest request)
        {

            var command = _mapper.Map<UpdateProfileCommand>(request);
            command.OwnerUserId = userId;
            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);
            return Ok(result);
        }

    }
}
