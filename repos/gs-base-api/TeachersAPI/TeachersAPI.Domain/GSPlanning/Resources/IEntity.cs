using System;

namespace GSPlanning.Domain
{
	public interface IEntity
	{
		int Id { get; set; }
		int ModifiedBy { get; set; }
		DateTime? CreatedAt { get; set; }
		DateTime? UpdatedAt { get; set; }
	}
}
