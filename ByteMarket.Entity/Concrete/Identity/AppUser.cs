

using Microsoft.AspNetCore.Identity;

namespace ByteMarket.Entities.Concrete.Identity
{
	public class AppUser : IdentityUser<Guid>
	{
		public string NameSurname { get; set; }

		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenEndDate { get; set; }

		public ICollection<Basket> Baskets { get; set; }
	}
}
