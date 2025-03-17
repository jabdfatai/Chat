using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.Response
{
    public class AddResponse
    {
        public AddResponse(string miscField1, string miscField2, bool? successful = false)
        {
            MiscField1 = miscField1;
            MiscField2 = miscField2;
            Successful = successful;
        }
        public AddResponse()
        {

        }
        public string MiscField1 { get; set; }
        public string MiscField2 { get; set; }
        public object Warning { get; set; }
        public bool? Successful { get; set; }
    }
}
