using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{

    public static class ValidatorIncomingMessage
    {
        public static void CheckEOF(string incomingByte, XmlFilter xmlFilter)
        {
            string asd = xmlFilter.GetEOF();
        }
    }
}
