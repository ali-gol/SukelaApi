using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SukelaApi.Service;
using SukelaApi.Service.Model;
using SukelaApi.WebApi.ViewModel;
using System.Threading.Tasks;

namespace SukelaApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly LoginService _loginService;
        public AuthController()
        {
            _loginService = new LoginService();
        }
        [AllowAnonymous]
        [HttpPost]
        public Task<BaseResponse<string>> Login(LoginRequest model)
        {
            return _loginService.Login(model.UserName, model.Password);
        }
    }
}
