using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pamaxie.Database.Redis.DataClasses;
using PamaxieML.Api.Data;
using PamaxieML.Model;

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
        public ActionResult<string> ProbeApiTask()
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
            var reader = new StreamReader(Request.Body);
            var result = reader.ReadToEndAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(result)) return BadRequest(ErrorHandler.BadData());
            var filehash = await ImageProcessing.GetFileHash(result);
            MediaPredictionData data = new MediaPredictionData(filehash);
            if (data.TryLoadData(out var knownResult))
            {
                return JsonConvert.SerializeObject(knownResult);
            }


            var image = ImageProcessing.DownloadFile(result);
            // Add input data
            var input = new ModelInput
            {
                ImageSource = image.FullName
            };
            // Load model and predict output of sample data
            ConsumeModel.Predict(input, out var labelResult);

            var predictionData = new MediaData()
            {
                DetectedLabels = labelResult
            };

            data.Data = predictionData;
            image.Delete();
            //scanFile.Dispose();
            // Create the response
            return JsonConvert.SerializeObject(labelResult);
        }
    }
}