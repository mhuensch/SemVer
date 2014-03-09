using System;

namespace Run00.TestSample
{
	/// <summary>
	/// Represents an line item in the system.
	/// </summary>
	public class LineItem
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
	}
}
