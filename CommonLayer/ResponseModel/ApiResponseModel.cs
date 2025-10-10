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
}
