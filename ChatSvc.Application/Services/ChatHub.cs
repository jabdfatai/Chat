using Microsoft.AspNetCore.SignalR;
using SecureCommSvc.Application.Services.Interface;
using SecureCommSvc.Core.Entity;
using SecureCommSvc.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Application.Services
{

    public class ChatHub : Hub
    {
   
        public ChatHub()
        {
           
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
