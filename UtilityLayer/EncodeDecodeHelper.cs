using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLayer
{
    public  class EncodeDecodeHelper
    {
        public static string EncodeDataToBase64(string data)
        {
            try
            {
                byte[] encData_byte= new byte[data.Length];
                encData_byte=System.Text.Encoding.UTF8.GetBytes(data);
                string encodeData=Convert.ToBase64String(encData_byte);
                return encodeData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode"+ex.Message);
            }
        }
        public static string DecodeDataToBase64(string encodeData)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            System.Text.Decoder decoder = encoding.GetDecoder();
            byte[] todecode_byte=Convert.FromBase64String(encodeData);
            int charcount= decoder.GetCharCount(todecode_byte,0, todecode_byte.Length);
            char[] decode_char= new char[charcount];
            decoder.GetChars(todecode_byte, 0, todecode_byte.Length,decode_char,0);
            string result=new string(decode_char);
            return result;
        }
    }
}
