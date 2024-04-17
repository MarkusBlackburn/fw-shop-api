using fw_shop_api.Models.Enums;
using fw_shop_api.Models.Util;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace fw_shop_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult ReturnResponse(dynamic model)
        {
            if (model.Status == RequestExecution.Successful) return Ok(model);

            return BadRequest(model);
        }

        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            BaseResponse<string> rsp = new BaseResponse<string>();
            rsp.Status = RequestExecution.Error;

            #if DEBUG
            rsp.Errors = new List<string>() {$"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}"};
            return BadRequest(rsp);

            #else 
            rsp.Errors = new List<string>() {"An error occured while processing your request"};
            return BadRequest(rsp);

            #endif
        }
    }
}