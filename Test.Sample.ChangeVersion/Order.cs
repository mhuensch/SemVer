using System;
using System.Runtime.Serialization;

namespace Run00.TestSample
{
	[DataContract]
	public class Order
	{
		[DataMember]
		public Guid Id { get; set; }
	}
}
