using Microsoft.AspNetCore.Mvc;

namespace tms_acl_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("API is running.");
        }
    }
}
