using System.Linq;


namespace Luxena.Travel.Domain
{

	partial class File
	{
		public class Service : EntityService<File>
		{

			public byte[] ContentBy(object id)
			{
				return Query.Where(a => a.Id == id).Select(a => a.Content).FirstOrDefault();
			}

		}
	}

}