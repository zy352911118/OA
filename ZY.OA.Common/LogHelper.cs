using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZY.OA.Common
{
   public class LogHelper
    {
        //记录异常的队列
        public static Queue<string> ExceptionStringQueue = new Queue<string>();
        private static ILog log = LogManager.GetLogger("Appender");
        static LogHelper()
        {

            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    if (ExceptionStringQueue.Count > 0)
                    {
                        //出队列
                        string Exceptionstring = ExceptionStringQueue.Dequeue();
                        log.Error(Exceptionstring);
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }

            });
            
        }
        public static void WriteLog(string Exception)
        {
            lock (ExceptionStringQueue)
            {  
                //异常消息存入队列
                ExceptionStringQueue.Enqueue(Exception);
            }
        }
    }
}
