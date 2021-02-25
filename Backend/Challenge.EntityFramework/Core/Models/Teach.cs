using Newtonsoft.Json;

#nullable disable

namespace Core.Models
{
	public class Teach
	{
		public int Id { get; set; }
		public int TeacherId { get; set; }
		public int CourseId { get; set; }

		[JsonIgnore] public virtual Course Course { get; set; }

		[JsonIgnore] public virtual Teacher Teacher { get; set; }
	}
}