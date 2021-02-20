using System;
using System.Collections.Generic;

#nullable disable

namespace Challenge_EF.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Teaches = new HashSet<Teach>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Teach> Teaches { get; set; }
    }
}
