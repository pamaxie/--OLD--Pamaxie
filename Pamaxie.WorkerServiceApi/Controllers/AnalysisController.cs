using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#pragma warning disable 1998 //TODO Remove when methods have been implemented (1998, Lacks await)

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
        /// Creates a new item in the queue to be analysed. Returns the url where the analysis will be put once ready.
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpPost("analyse")]
        public async Task<string> AnalyseTask()
        {
            //TODO: Fetch the last item in the queue
            return null;
        }

        /// <summary>
        /// Returns the next file the Worker services should analyse.
        /// </summary>
        /// <returns>oAuth Token</returns>
        [HttpPost("getLast")]
        public async Task<byte[]> GetLastTask()
        {
            //TODO: Fetch the last item in the queue
            return null;
        }
    }
}