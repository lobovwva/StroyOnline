using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StroyOnline_API.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        public short PositionId { get; set; }

        public int Salary { get; set; }
        public bool IsActive { get; set; }
    }
}
