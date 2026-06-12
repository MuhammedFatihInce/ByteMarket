using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteMarket.Business.DTOs.Order
{
	public class OrderItemDetailDto:OrderItemDto
	{
		public bool IsReviewed { get; set; }
		public string? ReviewId { get; set; }
		public string? Comment { get; set; }
		public int? Rating { get; set; }
	}
}
