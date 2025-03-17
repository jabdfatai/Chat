using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.ViewModel
{
    public class UserViewModel
        {
            public int ID { get; set; }
            public Guid USER_ID { get; set; }
            public string FIRSTNAME { get; set; }
            public string LASTNAME { get; set; }
            public DateTime? BIR_DT { get; set; }
            public string PHONE { get; set; }

            public string EMAIL { get; set; }

            public string? GENDER { get; set; }
            public string? TITLE { get; set; }
            public string? MARITALSTATUS { get; set; }

            public string? IDP_ENR { get; set; }
           
    }
}
