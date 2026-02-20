namespace ByteMarket.WebUI.Models.ResultModels
{
	public class ApiDataResponse<T>
	{
		public T Data { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
		public int StatusCode { get; set; }
	}
}
