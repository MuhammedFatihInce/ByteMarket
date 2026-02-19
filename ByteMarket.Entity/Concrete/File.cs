using ByteMarket.Entities.Common;


namespace ByteMarket.Entities.Concrete
{
	public class File : BaseEntity
	{
		public string FileName { get; set; }
		public string Path { get; set; }
		public string Storage { get; set; }
	}
}
