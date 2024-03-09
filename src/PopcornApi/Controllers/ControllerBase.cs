using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PopcornApi.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ControllerBase(IMediator mediator) : Controller
    {
        protected readonly IMediator mediator = mediator;
    }
}
