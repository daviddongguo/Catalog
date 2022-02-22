using Microsoft.AspNetCore.Mvc;

namespace David.Common.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
