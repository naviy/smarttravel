namespace Luxena.Travel.Domain
{

	partial class GdsFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class AirFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class MirFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class AmadeusXmlFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class GalileoXmlFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class SabreFilFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class LuxenaXmlFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}

	partial class GalileoRailXmlFileManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}

	}
	partial class GalileoBusXmlFileManager
	{
		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ReimportGdsDocuments", db.GdsFile.CanReimport() } };
		}
	}
}