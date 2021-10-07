using System.IO;
using System.Web;

using Luxena.Domain.Views;

using TwitterBootstrapMVC.Controls;


namespace Luxena.Domain.Web.UI
{

	public partial class DomainUiHelper<TModel>
	{

		public class GridHelper : UiHelper
		{
			public GridHelper(DomainUiHelper<TModel> ui) : base(ui) { }


			#region Buttons

			public BootstrapButton<TModel> AddButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "plus", null, "Добавить")
					.SuccessStyle()
					.ngClick("openNew()")
					.ngDisabled("loading || !canNew()");
			}

			public BootstrapButton<TModel> AddButton2(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "plus")
					.SuccessStyle()
					.ngClick("openNew()")
					.ngDisabled("loading || $parent.loading || !canNew()");

		//@bs.IconButton("plus", " ", style: ButtonStyle.Success, items: add => add(
		//	d => d.Link("Создать новый", null).ngClick("openNew()"),
		//	d => d.Link("Добавить существующий", null).ngClick("openLink()")
		//	))

			}

			public BootstrapButton<TModel> DeleteButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "trash-o", "Удалить {{selectedCount}} выделенных записей")
					.DangerStyle()
					.ngClick("delete()")
					.ngDisabled("loading || selectedCount() == 0");
			}

			public BootstrapButton<TModel> RefreshButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "refresh", "Обновить", initIcon: a => a.ngClass("{ 'fa-spin': loading }"))
					.ngDisabled("loading")
					.ngClick("refresh()");
			}

			public BootstrapButton<TModel> RowDeleteButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "trash-o")
					.DangerStyle()
					.ngClick("delete(r)")
					.ngDisabled("!canDelete(r)");
			}

			#endregion


			public BootstrapTextBox<TModel> TextSearch()
			{
				return bs.TextBox("search").ngModel("searchText")
					.Placeholder("Начните поиск")
					.CssStyle("width: 100%;");
			}


			#region Cell

			public IHtmlString Cell(ViewMember v, string attributes = null)
			{
				var w = new StringWriter();

				BlockBegin(w, v, "td", attributes);
				TextBody(w, v);
				BlockEnd(w, v, "td");

				return new HtmlString(w.GetStringBuilder().ToString());
			}

			public IHtmlString Cells(params ViewMember[] members)
			{
				var w = new StringWriter();

				foreach (var v in members)
				{
					BlockBegin(w, v, "td");
					TextBody(w, v);
					BlockEnd(w, v, "td");
				}

				return new HtmlString(w.GetStringBuilder().ToString());
			}

			#endregion


		}

	}

}
