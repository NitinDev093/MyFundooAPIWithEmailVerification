using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FundooAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserBusinessLayer userBusinessLayer;

        public UserController(IUserBusinessLayer _userBusinessLayer)
        {
            userBusinessLayer = _userBusinessLayer;
        }

        [Route("Adduser")]
        [HttpPost]
        public IActionResult InsertuserData([FromBody] UserRequestModel User) 
        {
            string token = Guid.NewGuid().ToString();
            //yeh token hum verifyurl ke saath isiliyea bhej rahe hai ki jab user verify karega
            //to yehi token mere EmailVerification api me aayega phir aage ka process hoga.
            string verifyUrl = $"{Request.Scheme}://{Request.Host}/api/User/EmailVerification?token={token}";
            var response = userBusinessLayer.InsertuserData(User,token,verifyUrl);
            return Ok(response);
        }

        [Route("EmailVerification")]
        [HttpGet]
        public IActionResult EmailVerification(string token)
        {
            var response=userBusinessLayer.EmailVerification(token);
            return Ok(response);
        }


    }
}
