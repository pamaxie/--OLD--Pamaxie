using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pamaxie.ImageScanning;

/*
namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Controller to analyse images
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class AnalysisController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ILogger<AnalysisController> _logger;

        public AnalysisController(ILogger<AnalysisController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Probes if the API is available, will probably be expanded with more option later, 
        /// mostly used for now for load balancers to check if everything is working
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpGet("Status")]
        [AllowAnonymous]
        public static ActionResult<string> ProbeApiTask()
        {
            //TODO: Check if the API is fully functional in an efficient way to prevent overload attacks via this request.
            return "Analysis API is available";
        }

        /// <summary>
        /// Verifies the content of a sent image
        /// </summary>
        /// <param name="fileStream">The file stream</param>
        /// <returns>oAuth Token</returns>
        [AllowAnonymous]
        [HttpPost("ScanImage")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ScanImageTask(string fileStream)
        {
            
            //TODO: FORWARD the request to the WorkerService or the analysis method directly depending on what the user sets (single server mode)
            //TODO: Allow sending/ analysis of binary/raw data
            //TODO: Add response for 102 Processing
            //TODO: DO NOT scan things directly on this controller, that's highly inefficient
            if (string.IsNullOrWhiteSpace(fileStream))
            {
                return BadRequest();
            }

            
            string filehash = await ImageProcessing.ImageProcessing.GetFileHash(fileStream);
            MediaPredictionData data = new MediaPredictionData(filehash);

            if (data.TryLoadData(out MediaData knownResult))
            {
                return Ok(JsonConvert.SerializeObject(knownResult));
            }

            FileInfo image = ImageProcessing.ImageProcessing.DownloadFile(fileStream);
            //Add input data
            ModelInput input = new ModelInput
            {
                ImageSource = image.FullName
            };
            //Load model and predict output of sample data
            ConsumeModel.Predict(input, out OutputProperties labelResult);

            MediaData predictionData = new MediaData
            {
                DetectedLabels = labelResult
            };

            data.Data = predictionData;
            image.Delete();
            //scanFile.Dispose();
            // Create the response
            return Ok(JsonConvert.SerializeObject(labelResult));
        }

        /// <summary>
        /// Gets any existing data already present in the database, if none exist we return 404 to tell the user it doesn't exist.
        /// </summary>
        /// <param name="fileStream">The file stream</param>
        /// <returns>oAuth Token</returns>
        [AllowAnonymous]
        [HttpPost("GetExistingData")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetExistingData(string fileStream)
        {
            if (string.IsNullOrWhiteSpace(fileStream))
            {
                return BadRequest();
            }

            string filehash = await ImageProcessing.ImageProcessing.GetFileHash(fileStream);
            MediaPredictionData data = new MediaPredictionData(filehash);

            if (data.TryLoadData(out MediaData knownResult))
            {
                return Ok(JsonConvert.SerializeObject(knownResult));
            }

            return NotFound("The file has not yet been analyzed by our system.");
        }

        /// <summary>
        /// Gets the Files File Hash
        /// </summary>
        /// <param name="fileStream">The file stream</param>
        /// <returns>oAuth Token</returns>
        [AllowAnonymous]
        [HttpPost("GetHash")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> GetHash(string fileStream)
        {
            if (string.IsNullOrWhiteSpace(fileStream))
            {
                return BadRequest();
            }

            string hash = await ImageProcessing.ImageProcessing.GetFileHash(fileStream);

            if (string.IsNullOrWhiteSpace(hash))
            {
                return BadRequest();
            }

            return Ok(hash);
        }
    }
}*/