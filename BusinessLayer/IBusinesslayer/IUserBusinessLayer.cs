using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IBusinesslayer
{
    public interface IUserBusinessLayer
    {
        ApiResponseModel<string> InsertuserData(UserRequestModel user,string token ,string verifyUrl);
        ApiResponseModel<string> EmailVerification(string token);
        ApiResponseModel<string> UserLogin(string email,string password);
        ApiResponseModel<dynamic> CheckEmailExistance(string email);
        ApiResponseModel<string> ForgotPassword(string email);
        ApiResponseModel<string> ResetPassword(int userId,string newPassword);
    }
}
