#nullable disable

namespace Challenge_EF.Models
{
	public class Teach
	{
		public int Id { get; set; }
		public int TeacherId { get; set; }
		public int CourseId { get; set; }

		public virtual Course Course { get; set; }
		public virtual Teacher Teacher { get; set; }
	}
}