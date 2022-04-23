using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
      //  public string ReaderId { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime DateTaken { get; set; }

       // [AllowNull]
        public DateTime? DateReturn { get; set; }

        public string SignatureId { get; set; }
        public virtual Signature Signature { get; set; }

        
       
    }
}
