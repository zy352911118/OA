using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.IDAL;

namespace ZY.OA.DALFactory
{
   public partial class AbstractDALFactory
    {
        private static readonly string AssemblyName = System.Configuration.ConfigurationManager.AppSettings["DalAssemblyName"];
        #region T4生成
        //public static IUserInfoDal GetUserInfoDal()
        //{
        //    return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".UserInfoDal") as IUserInfoDal;
        //}
        //public static IOrderInfoDal GetOrderInfoDal()
        //{
        //    return Assembly.Load(AssemblyName).CreateInstance(AssemblyName + ".OrderInfoDal") as IOrderInfoDal;
        //} 
        #endregion
    }
}
