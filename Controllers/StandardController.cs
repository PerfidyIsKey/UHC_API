
namespace UHC_API.Controllers
{
    [ApiController]
    [Route("")]
    public class StandardController : ControllerBase
    {
        [HttpGet(Name = "Standard")]
        public bool Post()
        {
            return true;
        }
    }
}