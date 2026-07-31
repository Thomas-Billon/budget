using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Api.Tests
{
    [Route("[controller]")]
    public class TestController : ApiControllerBase
    {
        [HttpGet("hello-world")]
        [AllowAnonymous]
        public async Task<IActionResult> HelloWorld()
        {
            return Ok(new { Result = "Hello, world!" });
        }
    }
}
