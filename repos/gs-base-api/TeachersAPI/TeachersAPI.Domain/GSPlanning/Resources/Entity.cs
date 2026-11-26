using System;

namespace GSPlanning.Domain
{
	public class Entity : IEntity
	{
		public int Id { get; set; }
		public int ModifiedBy { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
