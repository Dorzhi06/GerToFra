using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithCSV
{
    internal class CSVfile
    {
        public string VarName { set; get; }
        public string TimeString { set; get; }
        public string VarValue { set; get; }
        public string Validity { set; get; }
        public string Time_MS { set; get; }
        public string Time { set; get; }

        public CSVfile()
        {

        }
        public CSVfile(string varName, string timeString, string varValue, string validity,
            string time_ms, string time)
        {
            VarName = varName;
            TimeString = timeString;
            VarValue = varValue;
            Validity = validity;
            Time_MS = time_ms;
            Time = time;
        }
    }
}
