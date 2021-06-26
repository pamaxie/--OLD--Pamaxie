using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Pamaxie.Website.Services
{
    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        internal async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions").ConfigureAwait(false); //Never gets past this
        }
    }

    internal abstract class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
