using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithCSV
{
    internal class names
    {
        public string oldName { set; get; }
        public string newName { set; get; }
        public names(string oldname, string newname)
        {
            oldName = oldname;
            newName = newname;
        }
    }
}
