using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Shared.Models;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction] // bu metod bir api endpointi değil get/post işlemi yapmıyor.
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            if (response.StatusCode == 204 || response.Data == null && response.Message == null && response.Errors == null) // 204 : no content
                return new ObjectResult(null) { StatusCode = 204 };

            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}
