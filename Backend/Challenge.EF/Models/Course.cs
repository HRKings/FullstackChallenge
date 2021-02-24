using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Challenge.EF.Models
{
	public class Course
	{
		public Course()
		{
			Attends = new HashSet<Attend>();
			Teaches = new HashSet<Teach>();
		}

		public int Id { get; set; }
		public string Name { get; set; }

		[JsonIgnore] public virtual ICollection<Attend> Attends { get; set; }

		[JsonIgnore] public virtual ICollection<Teach> Teaches { get; set; }
	}
}