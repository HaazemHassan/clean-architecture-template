using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.API.Filters;
using Template.Application.Common.Pagination;
using Template.Application.Common.Responses;
using Template.Application.Features.Authentication.Commands.SignIn;
using Template.Application.Features.Authentication.Common;
using Template.Application.Features.Users.Commands.AddUser;
using Template.Application.Features.Users.Commands.Register;
using Template.Application.Features.Users.Commands.UpdateProfile;
using Template.Application.Features.Users.Queries.CheckEmailAvailability;
using Template.Application.Features.Users.Queries.GetUserById;
using Template.Application.Features.Users.Queries.GetUsersPaginated;

namespace Template.API.Controllers
{

    /// <summary>
    /// User management controller for handling user operations
    /// </summary>
    public class UserController : BaseController
    {


        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="command">User registration details including username, email, password, and personal information</param>
        /// <returns>JWT token with user information if registration is successful</returns>
        /// <response code="201">User registered successfully and JWT token returned</response>
        /// <response code="400">Invalid input data or registration failed</response>
        /// <response code="409">User with the same email, username, or phone number already exists</response>
        /// <response code="403">User is already authenticated (anonymous only endpoint)</response>
        [HttpPost("register")]
        [AnonymousOnly]
        [ProducesResponseType(typeof(Response<AuthResult>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Succeeded)
            {
                SignInCommand signInCommand = new SignInCommand
                {
                    Email = command.Email,
                    Password = command.Password
                };

                var signInResult = await Mediator.Send(signInCommand);
                if (signInResult.Succeeded)
                {
                    return CreatedAtAction(nameof(GetById), new { signInResult.Data!.User.Id }, result);
                }
                else
                {
                    // If sign-in fails after successful registration, return the registration result with a 201 status code
                    return CreatedAtAction(nameof(GetById), new { Id = result.Data!.Id }, result);
                }
            }
            return NewResult(result);
        }


        /// <summary>
        /// Get all users with pagination
        /// </summary>
        /// <param name="query">Pagination parameters including page number and page size</param>
        /// <returns>Paginated list of users with their details</returns>
        /// <response code="200">Returns paginated list of users</response>
        /// <response code="400">Invalid pagination parameters</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<GetUsersPaginatedQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] GetUsersPaginatedQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get user by their unique identifier
        /// </summary>
        /// <param name="query">User ID (int)</param>
        /// <returns>User details including username, email, name, address, and phone number</returns>
        /// <response code="200">Returns user details</response>
        /// <response code="404">User not found</response>
        /// <response code="400">Invalid user ID format</response>
        [HttpGet("{Id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(Response<GetUserByIdQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] GetUserByIdQuery query)
        {
            var result = await Mediator.Send(query);
            return NewResult(result);
        }




        /// <summary>
        /// Check if an email address is available for registration
        /// </summary>
        /// <param name="query">Email address to check</param>
        /// <returns>Boolean indicating whether the email is available</returns>
        /// <response code="200">Returns true if email is available, false otherwise</response>
        /// <response code="400">Invalid email format</response>
        [HttpGet("check-email/{Email}")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckEmailAvailability([FromRoute] CheckEmailAvailabilityQuery query)
        {
            var result = await Mediator.Send(query);
            return NewResult(result);
        }


        [HttpPost("add-user")]
        //[Authorize(Roles = $"{nameof(UserRole.SuperAdmin)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Succeeded)
                return CreatedAtAction(nameof(GetById), new { Id = result.Data!.Id }, result);

            return NewResult(result);
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
        [HttpPatch("profile")]
        [Authorize]
        [ProducesResponseType(typeof(Response<UpdateProfileCommandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var result = await Mediator.Send(command);
            return NewResult(result);
        }

    }
}
