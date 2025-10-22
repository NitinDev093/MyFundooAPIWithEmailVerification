using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepositoryLayer.IRepositoryLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IUserRepositoryLayer _userRepositoryLayer;
        private readonly EmailHelper _emailHelper;
        private readonly JwtHelper _jwtHelper;
        public UserBusinessLayer(EmailHelper emailHelper,JwtHelper jwtHelper, IUserRepositoryLayer userRepositoryLayer)
        {
            _userRepositoryLayer= userRepositoryLayer;
            _emailHelper= emailHelper;
            _jwtHelper= jwtHelper;
        }

        
        public ApiResponseModel<string> InsertuserData(UserRequestModel user,string token, string verifyUrl)
        {
            ApiResponseModel<string> response= new ApiResponseModel<string>();
            user.password= EncodeDecodeHelper.EncodeDataToBase64(user.password);
            int userId=_userRepositoryLayer.InsertuserData(user,token);
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
            DataTable userData = _userRepositoryLayer.EmailVerificationr(token);
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
            DataTable userData = _userRepositoryLayer.UserLogin(email, password);
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

        public ApiResponseModel<bool> CheckEmailExistance(string email)
        {
            ApiResponseModel<bool> response=new ApiResponseModel<bool>();
            DataTable status = _userRepositoryLayer.CheckEmailExistance(email);
            if (status.Rows.Count>0)
            {
                bool isSuccess = Convert.ToBoolean(status.Rows[0]["isExists"]);
                response.isSuccess = true;
                response.Data = isSuccess;
                response.message = "";
            }
            return response;
        }

        public ApiResponseModel<string> ForgotPassword(string email)
        {
            ApiResponseModel<string> response=new ApiResponseModel<string>();
            DataTable datatable = _userRepositoryLayer.CheckEmailExistance(email);
            if (datatable.Rows.Count > 0 && Convert.ToBoolean(datatable.Rows[0]["isExists"]))
            {
                DataTable UserData = _userRepositoryLayer.GetUserByEmailAddress(email);
                string token = _jwtHelper.GenerateToken(UserData, 30);
                string verifyUrl = $"http://localhost:4200/reset-password/{token}";
                string subject = "Reset Your Password";
                string body = $"Click on the given link to reset your password: <a href='{verifyUrl}'>Reset Password</a>";
                try
                {
                    _emailHelper.SendEmail(email, subject, body);
                    response.isSuccess = true;
                    response.message = "Your reset password link has been sent over the email. The reset password link will be valid till 30 minutes only.";
                }
                catch (Exception)
                {
                    response.isSuccess = false;
                    response.message = "Internal Server Error Please try again.";
                }
            }
            else
            {
                response.isSuccess= false;
                response.message = "You entered invalid email.";
            }
            return response;
        }

        public ApiResponseModel<string> ResetPassword(string newPassword)
        {
            ApiResponseModel<string> response=new ApiResponseModel<string>();
            newPassword= EncodeDecodeHelper.EncodeDataToBase64(newPassword);
            bool updatedPassword = _userRepositoryLayer.ResetPassword(newPassword);
            if (updatedPassword)
            {
                response.isSuccess = true;
                response.message = "Password reset successful";
            }
            else
            {
                response.isSuccess = false;
                response.message = "Unable to reset password";
            }
            return response;
        }
    }
}
