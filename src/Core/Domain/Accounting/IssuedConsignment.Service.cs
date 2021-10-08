using System.Linq;


namespace Luxena.Travel.Domain
{

	partial class IssuedConsignment
	{
		public class Service : EntityService<IssuedConsignment>
		{

			public byte[] GetFile(object id)
			{
				return Query
					.Where(a => a.Id == id)
					.Select(a => a.Content)
					.FirstOrDefault();
			}

		}
	}

}