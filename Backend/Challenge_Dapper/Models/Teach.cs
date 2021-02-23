using Newtonsoft.Json;

#nullable disable

namespace Challenge_Dapper.Models
{
	public class Teach
	{
		public int Id { get; set; }
		public int TeacherId { get; set; }
		public int CourseId { get; set; }
	}
}