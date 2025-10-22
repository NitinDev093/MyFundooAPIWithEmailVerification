using CommonLayer.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.ResponseModel
{
    public class ApiResponseModel<T>
    {
        public string message { get; set; }
        public bool isSuccess { get; set; }
        public T Data {  get; set; }
    }
    public class UserResponseModel:UserRequestModel     
    {
        public int userId { get; set; }
        public bool isActive { get; set; }
    }
    public class NotesResponseModel : NotesRequestModel
    {
        public int NoteId { get; set; }
        public string CreatedAt { get; set; }
    }
}
