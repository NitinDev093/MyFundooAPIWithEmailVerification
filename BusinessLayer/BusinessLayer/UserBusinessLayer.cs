using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using RepositoryLayer.IRepositoryLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLayer;

namespace BusinessLayer.BusinessLayer
{
    public class UserBusinessLayer : IUserBusinessLayer
    {
        private readonly IUserRepositoryLayer userRepositoryLayer;
        private readonly EmailHelper _emailHelper;
        public UserBusinessLayer(EmailHelper emailHelper, IUserRepositoryLayer _userRepositoryLayer)
        {
            userRepositoryLayer= _userRepositoryLayer;
            _emailHelper= emailHelper;
        }

        
        public ApiResponseModel<string> InsertuserData(UserRequestModel user,string token, string verifyUrl)
        {
            ApiResponseModel<string> response= new ApiResponseModel<string>();
            user.password= EncodeDecodeHelper.EncodeDataToBase64(user.password);
            int userId=userRepositoryLayer.InsertuserData(user,token);
            if (userId>0)
            {
                string subject = "Verify your email";
                string body = $"Hello {user.firstName},<br><br>" +
                              $"Please verify your account by clicking the link below:<br>" +
                              $"<a href='{verifyUrl}'>Verify Now</a><br><br>" +
                              $"This link will expire in 24 hours.";
                _emailHelper.SendEmail(user.emailAddress,subject,body);
                response.isSuccess= true;
                response.message = "Registration SuccessFull";
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
                if (isActive)
                {
                    _emailHelper.SendEmail(email, "Verifaication Successfull", "You Can login in your account");
                    response.isSuccess=true;
                    response.message = "Verifaication successfull";
                }
                else
                {
                    _emailHelper.SendEmail(email, "Token expired", "Please register again");
                    response.isSuccess = false;
                    response.message = "Unable to verify";
                }
            }
            return response;
        }


    }
}
