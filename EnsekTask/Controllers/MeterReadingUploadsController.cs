using EnsekTask.Models.Responses;
using EnsekTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EnsekTask.Controllers;

public class MeterReadingUploadsController(ILogger<MeterReadingUploadsController> logger, IFileProcessorService fileProcessingService) : Controller
{
    [HttpPost]
    [AllowAnonymous]
    [Route("/meter-reading-uploads")]
    [ProducesResponseType(typeof(MeterReadingUploadResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MeterReadingUploadResponse), (int)HttpStatusCode.FailedDependency)]
    public async Task<ActionResult<MeterReadingUploadResponse>> Index()
    {
        var result = new MeterReadingUploadResponse();

        //  I'm using the raw stream object for file processing rather
        //  than loading the body into a string, because it will use
        //  less memory and run quicker - turning it into a string
        //  would effectively mean processing it twice and require
        //  holding an in-memory copy of the entire file

        var body = Request.Body;

        try
        {
            await fileProcessingService.ProcessFileAsync(body, result);
        }
        catch (Exception ex)
        {
            result.Error = ex.Message;
            logger.LogError(ex, "Failed to process the uploaded file");
            return StatusCode((int)HttpStatusCode.FailedDependency, result);
        }

        return Json(result);
    }
}