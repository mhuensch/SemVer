using System.Collections.Generic;
using System.ServiceModel;

namespace Run00.NewNamespace
{
	[ServiceContract]
	public interface IOrderService
	{
		/// <summary>
		/// Gets the orders for the order service.
		/// </summary>
		/// <returns>All the orders in the service</returns>
		[OperationContract]
		IEnumerable<Order> GetOrders();
	}
}
