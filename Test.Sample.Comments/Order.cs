using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Run00.TestSample
{
	/// <summary>
	/// Represents an order in the system
	/// </summary>
	[DataContract]
	public class Order
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[DataMember]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the line items.
		/// </summary>
		/// <value>
		/// The line items.
		/// </value>
		[DataMember]
		public IEnumerable<LineItem> LineItems { get; set; }
	}
}
