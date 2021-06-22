using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pamaxie.Database.Redis.DataClasses;
using PamaxieML.Api.Data;
using PamaxieML.Model;
using System.IO;
using System.Threading.Tasks;

namespace PamaxieML.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VisionController : ControllerBase
    {
        private readonly ILogger<VisionController> _logger;

        public VisionController(ILogger<VisionController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Probes if the API is available, will probably be expanded with more option later, 
        ///     mostly used for now for load balancers to check if everything is working
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpGet("status")]
        [AllowAnonymous]
        public static ActionResult<string> ProbeApiTask()
        {
            return "API is available";
        }


        /// <summary>
        ///     Verifies the content of a sent image
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpPost("scanImage")]
        public async Task<ActionResult<string>> ScanImageTask()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());
            string filehash = await ImageProcessing.GetFileHash(result);
            MediaPredictionData data = new(filehash);
            if (data.TryLoadData(out MediaData knownResult))
            {
                return JsonConvert.SerializeObject(knownResult);
            }

            FileInfo? image = ImageProcessing.DownloadFile(result);
            // Add input data
            ModelInput input = new()
            {
                ImageSource = image?.FullName
            };
            // Load model and predict output of sample data
            ConsumeModel.Predict(input, out OutputProperties labelResult);

            MediaData predictionData = new()
            {
                DetectedLabels = labelResult
            };

            data.Data = predictionData;
            image?.Delete();
            //scanFile.Dispose();
            // Create the response
            return JsonConvert.SerializeObject(labelResult);
        }
    }
}