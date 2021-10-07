using System;

using NUnit.Framework;
using NUnit.Framework.Interfaces;


namespace Luxena.Travel.Domain.Tests
{

	using Travel.Domain;


	[TestFixture]
	public abstract class DomainTest
	{
		public Domain db;
		public string EntryMap { get; set; }

		protected void RecreateDb()
		{
			db.Dispose();
			db = new Domain();
		}
	}


	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	public class InitDomainAttribute : Attribute, ITestAction
	{

		public bool UseTruncateDatabase { get; set; }


		public void BeforeTest(ITest testDetails)
		{
			if (testDetails.IsSuite) return;

			var test = (DomainTest)testDetails.Fixture;

			var db = test.db = new Domain();
			db.OnCalculated += () => test.EntryMap = db.EntryMap;

			if (UseTruncateDatabase)
				TruncateDatabase(db);
		}

		public void AfterTest(ITest testDetails)
		{
			if (testDetails.IsSuite) return;

			((DomainTest)testDetails.Fixture).db.Dispose();
		}

		public ActionTargets Targets => ActionTargets.Test | ActionTargets.Suite;


		public virtual void TruncateDatabase(Domain db) { }

	}


}