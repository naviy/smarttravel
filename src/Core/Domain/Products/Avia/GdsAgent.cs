using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Gds-агент", "Gds-агенты")]
	[SupervisorPrivileges]
	public partial class GdsAgent : Entity2
	{

		public virtual Person Person { get; set; }

		[RU("Источник документов")]
		public virtual ProductOrigin Origin { get; set; }

		[RU("Код агента"), EntityName]
		public virtual string Code { get; set; }

		[RU("Код офиса")]
		public virtual string OfficeCode { get; set; }

		[RU("Поставщик")]
		public virtual Organization Provider { get; set; }

		[Patterns.Owner, Suggest(typeof(DocumentOwner))]
		public virtual Party Office { get; set; }

		[Patterns.LegalEntity]
		public virtual Organization LegalEntity { get; set; }


		public override string ToString() => 
			Person?.ToString() ?? $"{OfficeCode} - {Code}";

	}
}