using System.Text.Json;

namespace ByteMarket.WebUI.Utilities.Helpers
{
	public static class JsonOptionsHelper
	{
		public static JsonSerializerOptions Default => new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true 
		};
	}
}
