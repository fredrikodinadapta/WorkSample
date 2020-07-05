using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkSampleExperis.Services;

namespace WorkSampleExperis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConvertController : ControllerBase
    {
        private readonly IConvertService _convertService;

        public ConvertController(IConvertService convertService) =>
            _convertService = convertService;

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Post()
        {
            try
            {
                var file = HttpContext.Request.Form.Files[0];
                if (file != null)
                {
                    var result = _convertService.Convert(file);
                    if (string.IsNullOrEmpty(result.Error)) { return Ok(result.XML); } else { return Ok(result.Error); }

                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ett fel inträffade: {ex.Message}");
            }
        }
    }
}
