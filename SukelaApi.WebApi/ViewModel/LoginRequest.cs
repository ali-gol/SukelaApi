using System.ComponentModel.DataAnnotations;

namespace SukelaApi.WebApi.ViewModel
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
