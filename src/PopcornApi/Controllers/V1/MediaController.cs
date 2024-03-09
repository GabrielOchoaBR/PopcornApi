using Application.V1.Dtos.Medias;
using Application.V1.Dtos.Shared;
using Application.V1.Features.Medias;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PopcornApi.Security.PolicyServices;
using Microsoft.AspNetCore.Http;

namespace PopcornApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class MediaController(IMediator mediator) : ControllerBase(mediator)
    {
        /// <summary>
        /// Get all media
        /// </summary>
        /// <param name="queryGetAll">Search parameters</param>
        /// <returns>List of media</returns>
        [Policy("Read")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseGetAll<MediaGetDto>>> GetAll([FromQuery] QueryGetAll queryGetAll)
        {
            return Ok(await mediator.Send(new GetAll.Query { QueryGetAll = queryGetAll }));
        }

        /// <summary>
        /// Gets a media by identificator
        /// </summary>
        /// <param name="id">Identity</param>
        /// <returns>Media information</returns>
        [Policy("Read")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MediaGetDto>> GetById(string id)
        {
            MediaGetDto? mediaGetDto = await mediator.Send(new GetById.Query { Id = id });

            if (mediaGetDto == null)
                return NotFound();

            return Ok(mediaGetDto);
        }

        /// <summary>
        /// Creates a new media
        /// </summary>
        /// <param name="mediaPostDto">Media information</param>
        /// <returns>Media information with new identity</returns>
        [Policy("Write")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MediaGetDto>> Post(MediaPostDto mediaPostDto)
        {
            var mediaGetDto = await mediator.Send(new Create.Command { MediaPostDto = mediaPostDto });

            if (mediaGetDto == null)
            {
                return BadRequest();
            }

            return Ok(mediaGetDto);
        }

        /// <summary>
        /// Updates a media information
        /// </summary>
        /// <param name="mediaPutDto">Media information</param>
        /// <returns>Media information changed</returns>
        [Policy("Write")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MediaGetDto>> Put(MediaPutDto mediaPutDto)
        {
            var mediaGetDto = await mediator.Send(new Update.Command { MediaPutDto = mediaPutDto });

            if (mediaGetDto == null)
            {
                return NotFound();
            }

            return Ok(mediaGetDto);
        }

        /// <summary>
        /// Updates specific media information
        /// </summary>
        /// <param name="id">Media identity</param>
        /// <param name="jsonPatchDocument">Update information</param>
        /// <returns>Media information changed</returns>
        [Policy("Write")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MediaGetDto?>> Patch(string id, JsonPatchDocument<MediaGetDto> jsonPatchDocument)
        {
            return Ok(await mediator.Send(new UpdatePartial.Command { Id = id, JsonPatchDocument = jsonPatchDocument }));
        }

        /// <summary>
        /// Deletes a media
        /// </summary>
        /// <param name="id">Media identity</param>
        /// <returns>Media information removed</returns>
        [Policy("Write")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MediaGetDto>> Delete(string id)
        {
            var mediaGetDto = await mediator.Send(new Delete.Command { Id = id });

            if (mediaGetDto == null)
            {
                return NotFound();
            }

            return Ok(mediaGetDto);
        }
    }
}
