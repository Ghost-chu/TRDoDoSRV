using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRDoDoSRV
{
    public class Util
    {
        public static string BytesToString(long? byteCount)
        {
            if (byteCount == null)
            {
                return "-1B";
            }

            long byteCountFinal = byteCount.Value;
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCountFinal);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCountFinal) * num).ToString() + suf[place];
        }
    }
}
