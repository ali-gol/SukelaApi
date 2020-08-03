using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SukelaApi.Service;
using SukelaApi.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SukelaApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaslikController : ControllerBase
    {
        //TODO: token bilgileri daha sonra header'dan alınacak.
        private readonly ILogger<BaslikController> _logger;
        public BaslikController(ILogger<BaslikController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Task<List<BaslikModel>> GetBugun(string token)
        {
            BaslikService baslikService = new BaslikService(token);
            return baslikService.GetBugun();
        }
        [HttpGet]
        public Task<List<BaslikModel>> GetGundem(string token)
        {
            BaslikService baslikService = new BaslikService(token);
            return baslikService.GetGundem();
        }
     
    }
}
