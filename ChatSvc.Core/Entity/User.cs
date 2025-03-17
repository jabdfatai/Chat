using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Core.Entity
{
    public class User: entityitem
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Key]
        public Guid USER_ID { get; set; }
        public string FST_NAME { get; set; }
        public string LST_NAME { get; set; }
        public DateTime? BIR_DT { get; set; }
        public int? TITLEID { get; set; }
        public int? MARITAL_STA_CODE { get; set; }
        public string? PHONE { get; set; }
        public string EMAIL { get; set; }
        public int? GENDER { get; set; }
        public string? IDP_ENR { get; set; }

    }
}
