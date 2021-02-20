using System;
using System.Collections.Generic;

#nullable disable

namespace Challenge_EF.Models
{
    public partial class Teach
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
