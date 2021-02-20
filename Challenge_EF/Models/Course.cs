using System;
using System.Collections.Generic;

#nullable disable

namespace Challenge_EF.Models
{
    public partial class Course
    {
        public Course()
        {
            Attends = new HashSet<Attend>();
            Teaches = new HashSet<Teach>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Attend> Attends { get; set; }
        public virtual ICollection<Teach> Teaches { get; set; }
    }
}
