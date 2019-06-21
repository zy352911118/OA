
using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.OA.Common
{
   public class memcachedHelper
    {
        public static MemcachedClient mc { get; set; }
        static memcachedHelper()
        {
            //获取Config里面的memcached的服务器地址配置
            string[] servers = ConfigurationManager.AppSettings["memcachedServerList"].Split(',');
            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(servers);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();
            //客户端实例
            mc = new Memcached.ClientLibrary.MemcachedClient();
            mc.EnableCompression = false;
        }

        /// <summary>
        /// 向Memcache里面写数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void set(string key,object value)
        {
            mc.Set(key, value);          
        }
        /// <summary>
        /// 向Memcache里面写数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime">过期时间</param>
        public static void set(string key, object value,DateTime expirationTime)
        {
            mc.Set(key, value, expirationTime);
        }

        /// <summary>
        /// 获取Memcache中的某条数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return mc.Get(key);
        }

        /// <summary>
        /// 删除Memcache中的某条数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
              return  mc.Delete(key);
            }
            return false;
        }
        /// <summary>
        /// 修改Memcache中的某条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        public static void Update(string key, object value, DateTime expirationTime)
        {
            if (mc.KeyExists(key))
            {
                mc.Delete(key);
            }
            mc.Set(key, value, expirationTime);
        }
    }
}
