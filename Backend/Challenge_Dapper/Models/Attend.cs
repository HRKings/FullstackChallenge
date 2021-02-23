using Newtonsoft.Json;

#nullable disable

namespace Challenge_Dapper.Models
{
	public class Attend
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public int StudentId { get; set; }
	}
}