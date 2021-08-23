using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pamaxie.Api.Data;
using PamaxieML.Model;
using System.IO;
using System.Threading.Tasks;
using Pamaxie.Database.Redis.DataClasses;

namespace Pamaxie.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ILogger<AnalysisController> _logger;

        public AnalysisController(ILogger<AnalysisController> logger)
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
            //TODO: Check if the API is fully functional in an efficient way to prevent overload attacks via this request.
            return "Analysis API is available";
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
            string filehash = await ImageProcessing.ImageProcessing.GetFileHash(result);
            MediaPredictionData data = new(filehash);
            if (data.TryLoadData(out var knownResult))
            {
                return JsonConvert.SerializeObject(knownResult);
            }

            FileInfo image = ImageProcessing.ImageProcessing.DownloadFile(result);
            // Add input data
            ModelInput input = new()
            {
                ImageSource = image.FullName
            };
            // Load model and predict output of sample data
            ConsumeModel.Predict(input, out var labelResult);

            MediaData predictionData = new()
            {
                DetectedLabels = labelResult
            };

            data.Data = predictionData;
            image.Delete();
            //scanFile.Dispose();
            // Create the response
            return JsonConvert.SerializeObject(labelResult);
        }

        /// <summary>
        ///     Gets any existing data already present in the database, if none exist we return 404 to tell the user it doesn't exist.
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpPost("getExistingData")]
        public async Task<ActionResult<string>> GetExistingData()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());
            string filehash = await ImageProcessing.ImageProcessing.GetFileHash(result);
            MediaPredictionData data = new(filehash);
            if (data.TryLoadData(out var knownResult))
            {
                return JsonConvert.SerializeObject(knownResult);
            }

            return NotFound("The file has not yet been analyzed by our system.");
        }

        /// <summary>
        ///     Gets the Files File Hash
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpPost("getHash")]
        public async Task<ActionResult<string>> GetHash()
        {
            StreamReader reader = new(Request.Body);
            string result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());
            return await ImageProcessing.ImageProcessing.GetFileHash(result);
        }
    }
}