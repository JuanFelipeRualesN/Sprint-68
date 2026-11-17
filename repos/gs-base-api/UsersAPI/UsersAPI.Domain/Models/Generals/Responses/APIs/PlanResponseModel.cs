using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
	/// <summary>
	/// A response with plan detail that includes more fields than PlanModel. It inherits from PlanModel.
	/// </summary>
	public class PlanResponseModel
	{
		/// <summary>
		/// Gets or sets the Id.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the platform id.
		/// </summary>
		public string PlatformId { get; set; }

		/// <summary>
		/// Gets or sets the product id.
		/// </summary>
		public string ProductId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		public string Interval { get; set; }

		/// <summary>
		/// Gets or sets the interval count.
		/// </summary>
		public string IntervalCount { get; set; }

		/// <summary>
		/// Gets or sets the currency.
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Gets or sets the amount.
		/// </summary>
		public string Amount { get; set; }

		/// <summary>
		/// Gets or sets if is for school ("1" yes, "0" no)
		/// </summary>
		public string IsForSchool { get; set; }

		/// <summary>
		/// Gets or sets if is private ("1" yes, "0" no)
		/// </summary>
		public string IsPrivate { get; set; }

		/// <summary>
		/// Gets or sets the profiles quantity.
		/// </summary>
		public string ProfilesQuantity { get; set; }

		/// <summary>
		/// Gets or sets if available or not ("1" yes, "0" no)
		/// </summary>
		public string Available { get; set; }

		/// <summary>
		/// Gets or sets the modified by.
		/// </summary>
		public string ModifiedBy { get; set; }

		/// <summary>
		/// Gets or sets the created at.
		/// </summary>
		public string CreatedAt { get; set; }

		/// <summary>
		/// Gets or sets the updated at. 
		/// </summary>
		public string UpdatedAt { get; set; }

		/// <summary>
		/// Gets or sets the trial days.
		/// </summary>
		public int TrialDays { get; set; }

		/// <summary>
		/// Gets or sets if the plan is available for ISO or not.
		/// </summary>
		public bool PlanAvailableForIso { get; set; }
	}
}