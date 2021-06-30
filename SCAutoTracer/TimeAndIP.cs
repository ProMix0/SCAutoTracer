using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAutoTracer
{
    public class TimeAndIP
    {
        public string Time { get; init; }
        public string IP { get; init; }

        public TimeAndIP(string time, string ip)
        {
            Time = time;
            IP = ip;
        }

        public override string ToString()
        {
            return Time;
        }
    }
}
