using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.OA.Model.SearchModel
{
   public class ActionInfoSearchParms:BaseParms
    {     
        public string Remark { get; set; }
        public string HttpMethd { get; set; }
        public string ActionName { get; set; }
    }
}
