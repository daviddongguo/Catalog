using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controller
{


    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
