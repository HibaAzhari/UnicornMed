using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicornMed.Common.Helpers
{
    public class DateHelper
    {
        public static bool IsOverlapping(DateTime s1, DateTime e1, DateTime s2, DateTime e2)
        {
            return (s1 > s2 && s1 < e2)
                || (e1 > s2 && e1 < e2)
                || (s1 < s2 && e1 > e2);
        }
    }
}
