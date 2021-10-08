using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;


namespace Luxena.Travel.Domain.Tests
{

	using Luxena.Domain;

	[TestFixture]
	public class SemanticTests
	{

		DefaultLocalizationTypesSource lng = new DefaultLocalizationTypesSource();
		Domain.EntityInfo[] entities;


		[TestFixtureSetUp]
		public void Init()
		{
			entities = Domain.CreateEntityInfos(lng, typeof(Entity2), typeof(Entity3), typeof(Entity3D));
			Domain.ConfigEntityInfos(entities);
		}


		[Test]
		public void Test_Find01()
		{
			var prop = typeof(Entity3).GetProperty("Name");
			Assert.NotNull(prop);

			Assert.NotNull(prop.Semantic<EntityNameAttribute>());
		}
	}

}
