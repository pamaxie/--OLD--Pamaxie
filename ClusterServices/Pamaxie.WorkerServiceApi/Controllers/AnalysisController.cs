using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pamaxie.Database.Extensions.Client;

namespace Pamaxie.Api.Controllers
{
    /// <summary>
    /// Controller to analyse images
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ILogger<AnalysisController> _logger;
        private readonly DatabaseService _dbService;

        public AnalysisController(ILogger<AnalysisController> logger, DatabaseService dbService)
        {
            _logger = logger;
            _dbService = dbService;
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