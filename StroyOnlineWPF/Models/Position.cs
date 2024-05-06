using System.Collections.Generic;

namespace StroyOnlineWPF.Models
{
    public class Position
    {
        public short Id { get; set; }
        public string PositionName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
