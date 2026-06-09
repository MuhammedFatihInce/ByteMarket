using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteMarket.Business.DTOs.Order
{
	public class InvoiceOrderDto:SingleOrderDto
	{
		public new IEnumerable<OrderItemDto> BasketItems { get; set; }
	}
}
