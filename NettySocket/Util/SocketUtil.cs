using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NettySocket.Util
{
    public class SocketUtil
    {

        public static readonly string Ascii2 = new string(new char[] { (char)2 });
	    public static readonly string Ascii3 = new string(new char[] { (char)3 });
        public static readonly string AsciiKey = new string(new char[] { (char)'$' });

        public static string PackMsg(string msg)
        {
            return Ascii2 + msg + Ascii3;
        }
        public static string BareMsg(string msg)
        {
            if (!string.IsNullOrEmpty(msg) && msg.StartsWith(Ascii2) && msg.EndsWith(Ascii3))
            {
                return Regex.Replace(msg,string.Format("[{0}{1}]",Ascii2,Ascii3),"");
            }
            return null;
        }
    }
}
