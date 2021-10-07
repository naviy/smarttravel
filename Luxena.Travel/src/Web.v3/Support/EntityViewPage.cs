using System.Web.Mvc;

using Luxena.Domain.Web.UI;

using TwitterBootstrap3;

using TwitterBootstrapMVC.BootstrapMethods.V3;


namespace Luxena.Travel
{

	public abstract class EntityViewPage<TModel> : WebViewPage<TModel>
	{

		public Bootstrap<TModel> bs { get; private set; }

		public DomainUiHelper<TModel> ui { get; private set; }

		public string Name { get { return ViewBag.ViewName; } set { ViewBag.ViewName = value; } }


		public override void InitHelpers()
		{
			base.InitHelpers();
			bs = Html.Bootstrap();
			ui = new DomainUiHelper<TModel>(Html);
			ViewBag.ui = ui;
		}

//		protected override void InitializePage()
//		{
//			base.InitializePage();
//
//			var sourceFiles = (HashSet<string>)_sourceFileProp.GetValue(PageContext);
//
//			var sourceFile = sourceFiles.LastOrDefault();
//			if (sourceFile != null)
//				Name = _reName.Match(sourceFile).By(1).As(name => name.Substring(0, 1).ToLower() + name.Substring(1));
//		}

	}


	public abstract class EntityViewPage : EntityViewPage<object>
	{

//		public static string GetDirectiveName(WebViewPage viewPage)
//		{
//			var sourceFiles = (HashSet<string>)_sourceFileProp.GetValue(viewPage.PageContext);
//			
//			var sourceFile = sourceFiles.LastOrDefault();
//			if (sourceFile != null)
//				return _reName.Match(sourceFile).By(1).As(name => name.Substring(0, 1).ToLower() + name.Substring(1));
//
//			return null;
//		}
//
//		static readonly PropertyInfo _sourceFileProp = typeof(WebPageContext).GetProperty("SourceFiles", BindingFlags.NonPublic | BindingFlags.Instance);
//		static readonly Regex _reName = new Regex(@"([\w\d_]+?)\.cshtml", RegexOptions.Compiled);
	}

}
