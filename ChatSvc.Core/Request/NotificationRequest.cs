using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.Request
{
    public sealed class NotModel
    {
        public Guid user_id { get; set; }
        public int eventid { get; set; }
        public string? subject { get; set; }
        public string body { get; set; }
        public SmsTo? smsTo { get; set; }
        public IList<To> To { get; set; } = new List<To>();
        public IList<To> CopyTo { get; set; } = new List<To>();
        public IList<To> BlindCopyTo { get; set; } = new List<To>();
        public IList<File> Files { get; set; } = new List<File>();
    }
    public record To
    {
        public string Address { get; set; }
        public string Display { get; set; }
    }
    public record SmsTo
    {
        public string PhoneNumber { get; set; }
    }
    public sealed record File
    {
        public string Name { get; set; }

        public byte[] Bytes { get; set; }

        public string ContentType
        {
            get
            {
                new FileExtensionContentTypeProvider().TryGetContentType(Name, out var contentType);

                return contentType;
            }
        }
    }
}
