using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class RetObj
    {
        public Dictionary<string, object> RetDics = new Dictionary<string, object>();
        public RetObj(string msg)
        {
            RetDics.Add("state",false);
            RetDics.Add("info", msg);
        }
        public RetObj(bool state , string msg)
        {
            RetDics.Add("state", state);
            RetDics.Add("info", msg);
        }

        public RetObj()
        {
            RetDics.Add("state", false);
            RetDics.Add("info", "");
        }
    }
}