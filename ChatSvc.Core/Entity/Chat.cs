using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.Entity
{
    public class Chat: entityitem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     
        public int id { get; set; }
        [Key]
        public Guid chatid { get; set; }
        public Guid sessionid { get; set; }
        public Guid sndrid { get; set; }
        public Guid rcvid { get; set; }
        public DateTime? senddate { get; set; }
        public string? sendtime { get; set; }
        public DateTime? readdate { get; set; }
        public string? readtime { get; set; }
        public string content { get; set; }
        public int chann_id { get; set; }
        public bool send_sta { get; set; }
        public bool del_sta { get; set; }
        public bool read_sta { get; set; }
        public Guid? replytoid { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
    public class ChatPatchModel
    {
       
        public DateTime? senddate { get; set; }
        public string? sendtime { get; set; }
        public DateTime? readdate { get; set; }
        public string? readtime { get; set; }
        public bool? send_sta { get; set; }
        public bool? del_sta { get; set; }
        public bool? read_sta { get; set; }

    }
}
