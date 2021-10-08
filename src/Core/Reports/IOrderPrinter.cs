using System.Collections.Generic;
using System.IO;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public interface IOrderPrinter
	{
		void Build(Stream stream, IList<Order> tickets);
	}
}