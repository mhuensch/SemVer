using System.Collections.Generic;
using System.ServiceModel;

namespace Run00.TestSample
{
	/// <summary>
	/// A  service for managing the orders in the system
	/// </summary>
	[ServiceContract]
	public interface IOrderService
	{
		/// <summary>
		/// Gets the orders for the order service and returns them as a list.
		/// </summary>
		/// <returns>All the orders in the service</returns>
		[OperationContract]
		IEnumerable<Order> GetOrders();
	}
}
