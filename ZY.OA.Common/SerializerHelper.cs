using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.OA.Common
{
   public class SerializerHelper
    {
        /// <summary>
        /// 进行序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializerToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 进行反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeSerializerToObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
