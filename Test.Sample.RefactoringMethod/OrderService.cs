using System.Collections.Generic;
using System.Linq;

namespace Run00.TestSample
{
	public class OrderService : IOrderService
	{
		public enum OrderType { None, Customer, Business }

		public IEnumerable<Order> GetOrders()
		{
			//Silly Refactor
			var list = new List<Order>();
			if (list.Count() == 0)
				return list;

			return Enumerable.Empty<Order>();
		}
	}
}
