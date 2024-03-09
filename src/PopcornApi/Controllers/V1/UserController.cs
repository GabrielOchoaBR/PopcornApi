using Application.V1.Dtos.Admin.Users;
using Application.V1.Features.Users;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PopcornApi.Security.PolicyServices;

namespace PopcornApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class UserController(IMediator mediator) : ControllerBase(mediator)
    {
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of users</returns>
        [Policy("Read")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserGetDto>>> GetAll()
        {
            return Ok(await mediator.Send(new GetAll.Query()));
        }
        /// <summary>
        /// Gets user by identity
        /// </summary>
        /// <param name="id">Identity</param>
        /// <returns>User information</returns>
        [Policy("Read")]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<UserGetDto>>> GetById(string id)
        {
            UserGetDto? userGetDto = await mediator.Send(new GetById.Query() { Id = id });

            if (userGetDto == null)
                return NotFound();

            return Ok(userGetDto);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userPostDto">User information</param>
        /// <returns>User information with new identity</returns>
        [AllowAnonymous]
        [EnableRateLimiting("LimitedAttempted")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserGetDto>> Post(UserPostDto userPostDto)
        {
            UserGetDto? userGetDto = await mediator.Send(new Create.Command() { UserPostDto = userPostDto });

            if (userGetDto == null)
                return BadRequest();

            return Ok(userGetDto);
        }

        /// <summary>
        /// Updates user information based on user identity
        /// </summary>
        /// <param name="userPutDto">New user information</param>
        /// <returns>User information</returns>
        [Policy("Write")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserGetDto>> Put(UserPutDto userPutDto)
        {
            UserGetDto? userGetDto = await mediator.Send(new Update.Command() { UserPutDto = userPutDto });

            if (userGetDto == null)
                return NotFound();

            return Ok(userGetDto);
        }

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">Identity</param>
        /// <returns>User information</returns>
        [Policy("Write")]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserGetDto>> Delete(string id)
        {
            UserGetDto? userGetDto = await mediator.Send(new Delete.Command() { Id = id });

            if (userGetDto == null)
                return NotFound();

            return Ok(userGetDto);
        }
    }
}
