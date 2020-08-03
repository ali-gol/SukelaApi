using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SukelaApi.Service;
using SukelaApi.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SukelaApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LinkController : ControllerBase
    {
        private readonly ILogger<BaslikController> _logger;

        public LinkController(ILogger<BaslikController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Task<List<Entry>> GetEntries(string url,string token)
        {
            var linkService = new LinkService(token);
            return linkService.GetDetail(url);
        }
    }
}
