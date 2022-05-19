using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/create-user")]
        public ActionResult<Result> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            var result = _userService.CreateUser(name, email, address, phone, userType, money);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

    }
}
