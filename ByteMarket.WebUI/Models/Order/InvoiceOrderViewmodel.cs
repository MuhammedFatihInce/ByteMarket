namespace ByteMarket.WebUI.Models.Order
{
	public class InvoiceOrderViewmodel:SingleOrderViewModel
	{
		public new IEnumerable<OrderItemViewModel> BasketItems { get; set; }
	}
}
