using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZY.OA.Common
{
   public class MD5Helper
    {
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5String(string str)
        {
            MD5 md5 = MD5.Create();
           byte[] buffer= Encoding.UTF8.GetBytes(str);
           byte[] Md5Buffer= md5.ComputeHash(buffer, 0, buffer.Length);
            StringBuilder sb = new StringBuilder();
            foreach (var b in Md5Buffer)
            {
                sb.Append(b.ToString("x2"));
            }
            md5.Clear();
            return sb.ToString();
        }
    }
}
