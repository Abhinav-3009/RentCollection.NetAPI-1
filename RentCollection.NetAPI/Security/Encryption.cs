using System;
using System.Text;

namespace RentCollection.NetAPI.Security
{
    public class Encryption
    {
        public static string Encrypt(string value)
        {

            try
            {
                byte[] encDataByte = new byte[value.Length];
                encDataByte = Encoding.UTF8.GetBytes(value);
                string encodedData = Convert.ToBase64String(encDataByte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public static string Decrypt(string encodedData)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Decoder utf8Decode = encoder.GetDecoder();
            byte[] toDecodeByte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(toDecodeByte, 0, toDecodeByte.Length);
            char[] decodedChar = new char[charCount];
            utf8Decode.GetChars(toDecodeByte, 0, toDecodeByte.Length, decodedChar, 0);
            string result = new String(decodedChar);
            return result;
        }
    }
}

