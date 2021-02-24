using Newtonsoft.Json;

#nullable disable

namespace Challenge.EF.Models
{
	public class Attend
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public int StudentId { get; set; }

		[JsonIgnore] public virtual Course Course { get; set; }

		[JsonIgnore] public virtual Student Student { get; set; }
	}
}