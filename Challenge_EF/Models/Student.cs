using System;
using System.Collections.Generic;

#nullable disable

namespace Challenge_EF.Models
{
    public partial class Student
    {
        public Student()
        {
            Attends = new HashSet<Attend>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Attend> Attends { get; set; }
    }
}
