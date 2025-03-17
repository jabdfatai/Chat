using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.QueryParameters
{
    public class ChatQueryParameter: QueryStringParameters
    {
        public Guid? senderid { get; set; }
        public Guid? receiverid { get; set; }
        public Guid? userid { get; set; }
        public Guid? sessionid { get; set; }
        public string? content { get; set; }
        public bool? read_sta { get; set; }
    }

    public class ChatSpecificParameter : QueryStringParameters
    {
        public Guid sessionid { get; set; }
    }

}
