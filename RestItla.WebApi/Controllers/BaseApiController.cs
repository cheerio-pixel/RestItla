using Microsoft.AspNetCore.Mvc;

namespace RestItla.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}