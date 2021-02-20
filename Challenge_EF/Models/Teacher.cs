using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Challenge_EF.Models
{
	public class Teacher
	{
		public Teacher()
		{
			Teaches = new HashSet<Teach>();
		}

		public int Id { get; set; }
		public string Name { get; set; }

		[JsonIgnore] public virtual ICollection<Teach> Teaches { get; set; }
	}
}