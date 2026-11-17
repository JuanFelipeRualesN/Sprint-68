using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Lessons
{
	public class QuizMatchInstructionsModel : IResponse
	{
	 	public bool Valid { get; set; }
	 	public System.Collections.Generic.List<string> Messages { get; set; }
	 	public string Id { get; set; }
	 	public string QuizId { get; set; }
	 	public string MatchGroupNumber { get; set; }
	 	public string Instruction { get; set; }
	 	public string ModifiedBy { get; set; }
	 	public string CreatedAt { get; set; }
	 	public string UpdatedAt     { get; set; }

	 }
}