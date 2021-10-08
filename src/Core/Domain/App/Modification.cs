using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("���������", "������� ���������")]
	[SupervisorPrivileges(Create = null, Update = null, Copy = null, Delete = null)]
	public partial class Modification : Entity
	{

		[RU("�����"),  DateTime2]
		public virtual DateTime TimeStamp { get; set; }

		[RU("�����")]
		public virtual string Author { get; set; }

		[RU("���")]
		public virtual ModificationType Type { get; set; }

		public virtual string InstanceType { get; set; }

		public virtual string InstanceId { get; set; }

		[RU("������")]
		public virtual string InstanceString { get; set; }

		public virtual string Comment { get; set; }

		// ReSharper disable once ConvertToAutoProperty
		public virtual IDictionary<string, string> Items => _items;
		private readonly IDictionary<string, string> _items = new Dictionary<string, string>();


		[RU("��������� ��������")]
		public virtual string ItemsJson
		{
			get
			{
				return Items?
					.Select(a => $"['{a.Key}', '{a.Value}']")
					.Join(",")
					.As(a => $"[{a}]");
			}
		}



		public class Service : EntityService<Modification>
		{

		}

	}



}