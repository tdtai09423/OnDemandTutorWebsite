using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ODTDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("protected-resource")]
        public IActionResult GetProtectedResource()
        {
            return Ok(new { message = "This is a protected resource" });
        }
    }
}
