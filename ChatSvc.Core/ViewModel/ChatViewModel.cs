using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.ViewModel
{
    public class ChatListViewModel
    {
        public Guid sessionid { get; set; }
        public Guid correspondenceid { get; set; }

        public List<ChatViewModel> chatlist { get; set; }
        public int unreadmsgcount { get; set; }
        public int msgcount { get; set; }
    }
    public class ChatViewModel
    {
      
        public Guid chatid { get; set; }
        public Guid senderid { get; set; }
        public Guid receiverid { get; set; }
        public Guid sessionid { get; set; }
        public DateTime? senddate { get; set; }
        public string? sendtime { get; set; }
        public DateTime? readdate { get; set; }
        public string? readtime { get; set; }
        public string content { get; set; }
        public int chann_id { get; set; }
        public bool? send_sta { get; set; }
        public bool? del_sta { get; set; }
        public bool? read_sta { get; set; }
        public string msg_type { get; set; }

        public int rcvunreadcount { get; set; }
    }



    public class ChatAddViewModel
    {
        public Guid? sessionid { get; set; }
        public Guid senderid { get; set; }
        public Guid receiverid { get; set; }
        public string senderfirstname { get; set; }
        public string content { get; set; }
        public int chann_id { get; set; }
        public bool send_sta { get; set; }
      
    }
    public class ChatPatchViewModel
    {
        public DateTime? senddate { get; set; }
        public string? sendtime { get; set; }
        public DateTime? readdate { get; set; }
        public string? readtime { get; set; }
        public string? content { get; set; }
        public int? chann_id { get; set; }
        public bool send_sta { get; set; }
        public bool del_sta { get; set; }
        public bool read_sta { get; set; }
    }


}
