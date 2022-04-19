using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Entities
{
    public class TakeBook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string ReaderId { get; set; }

        public virtual Employee Employee { get; set; }
        public string EmoployeeId { get; set; }

        public DateTime DateTaken { get; set; }

        public virtual Signature Signature { get; set; }
        public string SignatureId { get; set; }
    }
}
