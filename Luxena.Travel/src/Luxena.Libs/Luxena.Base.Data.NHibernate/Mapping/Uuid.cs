using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;


namespace Luxena.Base.Data.NHibernate.Mapping
{

	public class Uuid : IGeneratorDef
	{
		public static readonly IGeneratorDef Generator = new Uuid();


		private Uuid() { }

		public string Class => "uuid.hex";

		public object Params => new { format = "N" };

		public System.Type DefaultReturnType => typeof(string);

		public bool SupportedAsCollectionElementId => true;

		public static Action<IIdMapper> Mapping = m =>
		{
			m.Generator(Generator);
			m.Type((IIdentifierType)NHibernateUtil.AnsiString);
			m.Length(32);
		};

	}

}