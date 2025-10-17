using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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

        [Route("userRegistration")]
        [HttpPost]
        public IActionResult InsertuserData([FromBody] UserRequestModel User) 
        {
            string token = Guid.NewGuid().ToString();
            string verifyUrl = $"{Request.Scheme}://{Request.Host}/api/User/EmailVerification?token={token}";
            var response = userBusinessLayer.InsertuserData(User,token,verifyUrl);
            return Ok(response);
        }

        [Route("EmailVerification")]//for activatiing the user account
        [HttpGet]
        public IActionResult EmailVerification(string token)
        {
            var response=userBusinessLayer.EmailVerification(token);
            return Ok(response);
        }

        [Route("UserLogin")]
        [HttpPost]
        public IActionResult UserLogin(string email,string password)
        {
            
            var response = userBusinessLayer.UserLogin(email, password);
            return Ok(response);
        }

        [Route("CheckEmailExistance")]
        [HttpPost]
        public IActionResult CheckEmailExistance(string email)
        {
            var response = userBusinessLayer.CheckEmailExistance(email);
            return Ok(response);
        }


        [Route("ForgotPassword")]
        [HttpGet]
        public IActionResult ForgotPassword(string email)
        {
            var response = userBusinessLayer.ForgotPassword(email);
            return Ok(response);
        }

        [Route("ResetPassword")]
        [HttpGet]
        public IActionResult ResetPassword(string newPassword)
        {
            var response = userBusinessLayer.ResetPassword(newPassword);
            return Ok(response);
        }




    }
}
