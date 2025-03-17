using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserIdentitySvc.Core.Interface
{
    public interface IEventStreamProvider
    {
        Task<bool> SendOrderRequest(string topic, string message, string serveraddress);
    }
}
