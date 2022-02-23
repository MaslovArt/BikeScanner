using Microsoft.AspNetCore.Mvc;

namespace BikeScanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {

    }
}

