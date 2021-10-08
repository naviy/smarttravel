using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

using Luxena.Domain.Views;

using TwitterBootstrap3;

using TwitterBootstrapMVC.BootstrapMethods.V3;
using TwitterBootstrapMVC.Controls;


namespace Luxena.Domain.Web.UI
{

	public partial class DomainUiHelper<TModel>
	{
		public readonly HtmlHelper<TModel> Html;

		public GridHelper Grid { get; private set; }

		public FormHelper Form { get; private set; }

		protected Bootstrap<TModel> bs;
		protected TextWriter Writer { get { return Html.ViewContext.Writer; } }

		public ViewEntity ViewEntity;

		public DomainUiHelper(HtmlHelper<TModel> html)
		{
			Html = html;
			bs = Html.Bootstrap();

			Grid = new GridHelper(this);
			Form = new FormHelper(this);
		}


		public abstract class UiHelper
		{
			protected readonly HtmlHelper<TModel> Html;
			protected readonly DomainUiHelper<TModel> ui;
			protected readonly Bootstrap<TModel> bs;

			protected UiHelper(DomainUiHelper<TModel> ui)
			{
				this.ui = ui;
				bs = ui.bs;
				Html = ui.Html;
			}


			public BootstrapButton<TModel> CloseButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "times", "Закрыть вкладку").ngClick("viewPage.close()");
				//.DangerStyle()
			}


			#region Text

			public IHtmlString Text(ViewMember v)
			{
				var w = new StringWriter();

				TextBegin(w, v);
				TextBody(w, v);
				TextEnd(w, v);

				return new HtmlString(w.GetStringBuilder().ToString());
			}

			protected void TextBegin(TextWriter w, ViewMember v)
			{
				if (v.IsReference)
				{
					w.Write(@"<a href=""");
					w.Write(EntityMvcHelper.GetViewUrl("{{r." + v + ".Type}}", "{{r." + v + ".Id}}"));
					w.Write(@""">");
				}
			}

			protected void TextBody(TextWriter w, ViewMember v)
			{
				w.Write("{{r.");
				w.Write(v.name);

				if (v.IsReference)
					w.Write(".Name");

				w.WriteLine("}}");
			}

			protected void TextEnd(TextWriter w, ViewMember v)
			{
				if (v.IsReference)
					w.Write("</a>");
			}

			#endregion


			#region Block

			public IHtmlString Block(ViewMember v, string tag, string attributes = null)
			{
				var w = new StringWriter();

				BlockBegin(w, v, tag, attributes);
				TextBody(w, v);
				BlockEnd(w, v, tag);

				return new HtmlString(w.GetStringBuilder().ToString());
			}

			protected void BlockBegin(TextWriter w, ViewMember v, string tag, string attributes = null)
			{
				w.Write("<");
				w.Write(tag);

				if (attributes.Yes())
				{
					w.Write(" ");
					w.Write(attributes);
				}

				if (v.IsName)
				{
					w.Write(@" ng-class=""{ 'btn-details': canView(r) }"" ng-click=""openView(r)""><big>");
				}
				else if (v.IsReference)
				{
					w.Write(@" ng-class=""{ 'btn-details': r.");
					w.Write(v);
					w.Write(@"}""");
					w.Write(@" ng-click=""app.openView({ name: r." + v + @".Type, id: r." + v + @".Id })""");
					w.Write(">");
				}
				else
					w.Write(">");
			}

			protected void BlockEnd(TextWriter w, ViewMember v, string tag)
			{
				if (v.IsName)
					w.Write("</big>");
				w.Write("</");
				w.Write(tag);
				w.WriteLine(">");
			}

			#endregion

		}


		public class HtmlBlock : IDisposable
		{
			protected readonly Action OnDisposeAction;

			public HtmlBlock(Action onDisposeAction)
			{
				OnDisposeAction = onDisposeAction;
			}

			public void Dispose()
			{
				OnDisposeAction();
			}
		}

	}

}
