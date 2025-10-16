using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using Newtonsoft.Json;
using RepositoryLayer.IRepositoryLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UtilityLayer;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer.BusinessLayer
{
    public class UserBusinessLayer : IUserBusinessLayer
    {
        private readonly IUserRepositoryLayer userRepositoryLayer;
        private readonly EmailHelper _emailHelper;
        private readonly JwtHelper _jwtHelper;
        public UserBusinessLayer(EmailHelper emailHelper,JwtHelper jwtHelper, IUserRepositoryLayer _userRepositoryLayer)
        {
            userRepositoryLayer= _userRepositoryLayer;
            _emailHelper= emailHelper;
            _jwtHelper= jwtHelper;
        }

        
        public ApiResponseModel<string> InsertuserData(UserRequestModel user,string token, string verifyUrl)
        {
            ApiResponseModel<string> response= new ApiResponseModel<string>();
            user.password= EncodeDecodeHelper.EncodeDataToBase64(user.password);
            int userId=userRepositoryLayer.InsertuserData(user,token);
            if (userId>0)
            {
                string subject = "Verify your email";
                string body = _emailHelper.emailbodyTemplate(user.firstName, verifyUrl);
                _emailHelper.SendEmail(user.emailAddress, subject, body);
                response.isSuccess = true;
                response.message = "Registration SuccessFull!Please verfiy ypur account through email ";
            }
            else
            {
                response.message = "Unable TO Register user";
                response.isSuccess = false;
            }
            return response;
        }
        public ApiResponseModel<string> EmailVerification(string token)
        {
            ApiResponseModel<string> response=new ApiResponseModel<string>();
            if (string.IsNullOrEmpty(token))
            {
                response.isSuccess = false;
                response.message = "Invalid token";
            }
            DataTable userData = userRepositoryLayer.EmailVerificationr(token);
            if (userData.Rows.Count>0)
            {
                string email = userData.Rows[0]["Email"].ToString();
                bool isActive = Convert.ToBoolean(userData.Rows[0]["isActive"]);
                string Firstname = userData.Rows[0]["FirstName"].ToString();
                if (isActive)
                {
                    string body = _emailHelper.emailSuccessfullTemplate(Firstname);
                    string subject = "Verification successfull";
                    _emailHelper.SendEmail(email, subject, body);
                    response.isSuccess=true;
                    response.message = "Verifaication successfull";
                }
                else
                {
                    string body = _emailHelper.emailFailureTemplate(Firstname);
                    string subject = "Token expired";
                    _emailHelper.SendEmail(email, subject, body);
                    response.isSuccess = false;
                    response.message = "Unable to verify";
                }
            }
            return response;
        }

        public ApiResponseModel<string> UserLogin(string email, string password)
        {
            ApiResponseModel<string> response=new ApiResponseModel<string>();
            password= EncodeDecodeHelper.EncodeDataToBase64(password);
            DataTable userData = userRepositoryLayer.UserLogin(email, password);
            if (userData.Rows.Count>0)
            {
                string token = _jwtHelper.GenerateToken(userData);
                response.isSuccess = true;
                response.message = "Login Successfull";
                response.Data = token;
            }
            else
            {
                response.isSuccess = false;
                response.message = "Invalid credentials";
                response.Data = null;
            }
            return response;
        }

        public ApiResponseModel<string> CheckEmailExistance(string email, string resetToken, string resetUrl)
        {
            ApiResponseModel<string> response=new ApiResponseModel<string>();
            bool isSuccess = userRepositoryLayer.CheckEmailExistance(email, resetToken);
            if (isSuccess)
            {
                string subject = "Reset your password";
                string body = _emailHelper.emailbodyTemplate("User", resetUrl);
                _emailHelper.SendEmail(email, subject, body);
                response.isSuccess = true;
                response.message = "Please check your email to reset your password";
            }
            else
            {
                response.isSuccess = false;
                response.message = "Invalid Email!Please enter a valid email that you use during signup?";
            }
            return response;
        }
    }
}
