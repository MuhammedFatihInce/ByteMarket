namespace ByteMarket.Business.Abstract.Storage
{
	public interface IStorageService:IStorage
	{
		public string StorageName { get; }
	}
}
