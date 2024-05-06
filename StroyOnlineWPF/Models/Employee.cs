using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StroyOnlineWPF.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }

        public DateTime Birthday { get; set; }

        public short? PositionId { get; set; }

        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; } 

        public int Salary { get; set; }
        public bool IsActive { get; set; }
    }
}
