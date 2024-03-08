using API.Error;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("erorr/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErorrController : ControllerBase
    {
        public IActionResult Erorr(int code)
        {
            return new ObjectResult(new FailResponse(code));
        }
    }
}