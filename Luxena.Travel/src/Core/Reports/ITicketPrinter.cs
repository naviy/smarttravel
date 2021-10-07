using System.Collections.Generic;
using System.IO;

using Luxena.Travel.Domain;

namespace Luxena.Travel.Reports
{
	public interface ITicketPrinter
	{
		void Build(Stream stream, IList<AviaTicket> tickets);
	}
}