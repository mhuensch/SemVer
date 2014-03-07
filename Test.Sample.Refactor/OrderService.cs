using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.TestSample
{
	public class OrderService : IOrderService
	{
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
