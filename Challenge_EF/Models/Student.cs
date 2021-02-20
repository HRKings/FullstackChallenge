using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<Attend> Attends { get; set; }
    }
}
