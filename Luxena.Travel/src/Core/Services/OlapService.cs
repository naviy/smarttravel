using Microsoft.AnalysisServices.AdomdClient;


namespace Luxena.Travel.Domain
{

	public class OlapService
	{
		public string ConnectionString { get; set; }

		public string Database { get; set; }

		public void Process()
		{
			using (var conn = new AdomdConnection(ConnectionString))
			{
				conn.Open();

				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = string.Format(@"<Process xmlns=""http://schemas.microsoft.com/analysisservices/2003/engine""><Type>ProcessFull</Type><Object><DatabaseID>{0}</DatabaseID></Object></Process>", Database);
					cmd.ExecuteNonQuery();
				}
			}
		}
	}


	partial class Domain
	{
		public OlapService Olap { get { return Resolve(ref _olap); } }
		private OlapService _olap;
	}

}