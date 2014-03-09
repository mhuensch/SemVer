using System.Collections.Generic;
using System.Linq;

namespace Run00.TestSample
{
	public class OrderService : IOrderService
	{
		public enum OrderType { None, Customer, Business }

		public IEnumerable<Order> GetOrders()
		{
			return Enumerable.Empty<Order>();
		}
	}
}
