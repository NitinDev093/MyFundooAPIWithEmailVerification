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

        [Route("Adduser")]
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

        [Route("CheckEmailExistance")]//In case of reset password or forgot password
        [HttpPost]
        public IActionResult CheckEmailExistance(string email)
        {
            string resetToken = Guid.NewGuid().ToString();
            string resetUrl = $"{Request.Scheme}://{Request.Host}/api/User/ResetPassword?token={resetToken}";
            var response = userBusinessLayer.CheckEmailExistance(email,resetToken,resetUrl);
            return Ok(response);
        }

        //public IActionResult ResetPassword(string resetToken)
        //{
        //    return Ok("sefsdgdfbdfb");
        //}




    }
}
