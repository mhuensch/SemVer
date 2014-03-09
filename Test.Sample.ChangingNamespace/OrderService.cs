using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.NewNamespace
{
	public class OrderService : IOrderService
	{
		public IEnumerable<Order> GetOrders()
		{
			return Enumerable.Empty<Order>();
		}
	}
}
