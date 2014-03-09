using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.TestSample
{
	public class OrderService : IOrderService<int>
	{
		public IEnumerable<Order> GetOrders()
		{
			return Enumerable.Empty<Order>();
		}
	}
}
