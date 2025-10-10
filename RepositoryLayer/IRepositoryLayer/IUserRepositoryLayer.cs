using CommonLayer.RequestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.IRepositoryLayer
{
    public interface IUserRepositoryLayer
    {
        int InsertuserData(UserRequestModel User,string token);
        DataTable EmailVerificationr(string token);
    }
}
