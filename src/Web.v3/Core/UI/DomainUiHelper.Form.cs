using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

using Luxena.Domain.Views;

using TwitterBootstrap3;

using TwitterBootstrapMVC;
using TwitterBootstrapMVC.BootstrapMethods;
using TwitterBootstrapMVC.ControlInterfaces;
using TwitterBootstrapMVC.Controls;

using InputType = Luxena.Domain.Views.InputType;


namespace Luxena.Domain.Web.UI
{

	public partial class DomainUiHelper<TModel>
	{

		public class FormHelper : UiHelper
		{
			public FormHelper(DomainUiHelper<TModel> ui) : base(ui) { }


			#region Buttons

			public BootstrapButton<TModel> EditButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "pencil", null, "Изменить")
					.PrimaryStyle()
					.ngDisabled("loading || !r || !canEdit()")
					.ngClick("openEdit()");
			}

			public BootstrapButton<TModel> DeleteButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "trash-o", "Удалить")
					.DangerStyle()
					.ngDisabled("loading || !r || !canDelete()");
			}

			public BootstrapButton<TModel> BackToViewButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "arrow-right", "Вернуться к просмотру")
					.ngClick("openView()");
				//.DangerStyle()
			}

			public BootstrapButton<TModel> RefreshButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "refresh", "Обновить")// , initIcon: a => a.ngClass("{ 'fa-spin': loading }"))
					.ngDisabled("loading")
					.ngClick("refresh()");
			}

			public BootstrapButton<TModel> SaveButton(ButtonGroupBuilder<TModel> g = null)
			{
				return bs.IconButton(g, "save", null, "Сохранить")// , initIcon: a => a.ngClass("{ 'fa-spin': loading }"))
					.PrimaryStyle()
					.ngDisabled("loading || saving || !form.$valid")
					.ngClick("save()");
			}

			#endregion


			#region Display

			protected void DisplayBegin(TextWriter w, ViewMember v)
			{
				var name = v.name;
				var title = v.title.Short;
				var tooltip = v.description ?? v.title.Long;


				w.Write(@"<div ng-show=""r.");
				w.Write(name);
				w.WriteLine(@""">");

				if (tooltip.No() || tooltip == title)
					tooltip = null;
				else
					tooltip = @" tooltip=""" + tooltip + @""" tooltip-placement=""left""";

				if (v.IsName)
				{
					var icon = ui.ViewEntity.CssIcon();
					if (icon.Yes())
					{
						w.Write(@"<dt><i class=""");
						w.Write(icon);
						w.Write(@" fa-2x""></i></dt>");
					}
					w.Write(@"<dd class=""lead"">");
				}
				else
				{
					w.Write("<dt");
					w.Write(tooltip);
					w.Write(">");
					w.Write(title);
					w.Write(":</dt>");
					w.Write("<dd>");
				}

				TextBegin(w, v);
			}

			protected void DisplayBody(TextWriter w, ViewMember v)
			{
				w.Write(@"<span style=""white-space: pre-wrap;"">");
				TextBody(w, v);
				w.WriteLine("</span>");
			}

			protected void DisplayEnd(TextWriter w, ViewMember v, bool appendBreak = false)
			{
				TextEnd(w, v);
				w.WriteLine("</dd>");
				if (appendBreak)
					w.WriteLine("<br/>");
				w.WriteLine("</div>");
			}

			public IHtmlString Display(ViewMember v, bool appendBreak = false)
			{
				var w = new StringWriter();
				DisplayBegin(w, v);
				DisplayBody(w, v);
				DisplayEnd(w, v, appendBreak);
				return new HtmlString(w.GetStringBuilder().ToString());
			}

			public IHtmlString DisplayLine(ViewMember v)
			{
				return Display(v, true);
			}

			public IHtmlString Displays(params ViewMember[] members)
			{
				var w = new StringWriter();

				foreach (var v in members)
				{
					DisplayBegin(w, v);
					DisplayBody(w, v);
					DisplayEnd(w, v);
				}

				return new HtmlString(w.GetStringBuilder().ToString());
			}

			public IHtmlString DisplayLines(params ViewMember[] members)
			{
				var w = new StringWriter();

				foreach (var v in members)
				{
					DisplayBegin(w, v);
					DisplayBody(w, v);
					DisplayEnd(w, v, appendBreak: true);
				}

				return new HtmlString(w.GetStringBuilder().ToString());
			}


			public HtmlBlock DisplayBegin(ViewMember v)
			{
				DisplayBegin(ui.Writer, v);

				return new HtmlBlock(() => DisplayEnd(ui.Writer, v));
			}
			
			#endregion


			#region Form

			public FormBuilder<TModel> BeginHForm(ViewEntity v, Action<Form> initForm = null)
			{
				return bs.Begin(new Form()
					.HtmlAttributes(new { name = "form" })
					.LabelWidthMd(4).LabelWidthLg(3)
					.Type(FormType.Horizontal).Do(initForm)
				);
			}


			public IHtmlString Label(ViewMember v, Action<IBootstrapLabel> initLabel = null)
			{
				return bs.Label(v).Do(a => InitLabel(a, v, initLabel));
			}

			void InitLabel(IBootstrapLabel lbl, ViewEntity v, Action<IBootstrapLabel> initLabel = null)
			{
				lbl.LabelHtml(new HtmlString(v.title.Short)).Do(initLabel);
			}


			public HtmlBlock Section(ViewEntity v)
			{
				SectionBegin(ui.Writer, v);

				return new HtmlBlock(() => SectionEnd(ui.Writer, v));
			}

			protected void SectionBegin(TextWriter w, ViewEntity v)
			{
				w.Write(@"<form-section title=""");
				w.Write(v.title.Short);
				w.Write(@""" description=""");
				w.Write(v.description);
				w.WriteLine(@""">");
			}
			protected void SectionEnd(TextWriter w, ViewEntity v)
			{
				w.WriteLine(@"</form-section>");
			}

			public delegate IHtmlString[] BootstrapRowItemsSetter(params IHtmlString[] items);
			public delegate IHtmlString[] BootstrapRowItemsGetter(BootstrapRowItemsSetter items);

			public IHtmlString Row<TM>(FormBuilder<TM> f, ViewMember v = null,
				BootstrapRowItemsGetter items = null,
				Action<IBootstrapLabel> initLabel = null)
			{
				var items_ = items == null ? null : items(a => a);

				if (items_.No()) return null;

				// ReSharper disable once PossibleNullReferenceException
				var newControls = new List<IHtmlString>(items_.Length + 2) { new HtmlString(@"<div class=""row"">") };
				newControls.AddRange(items_);
				newControls.Add(new HtmlString(@"</div>"));

				var c = @f.FormGroup().CustomControls(newControls.ToArray());
				return v == null ? c : c.CustomLabel(Label(v, initLabel));
			}

			#endregion


			#region Editors

			#region CheckBox

			public IHtmlString CheckBox<TM>(FormBuilder<TM> f, ViewMember v,
				Action<BootstrapControlGroupCheckBox<TM>> initEditor = null,
				Action<IBootstrapLabel> initLabel = null)
			{
				return f
					.FormGroup()
					.CheckBox(v)
						.Do(a => InitField(a.Class, a.HtmlAttributes, v))
						.Do(initEditor)
					.Label()
						.Do(a => InitLabel(a, v, initLabel));
			}

			public IHtmlString CheckBox(ViewMember v,
				Action<BootstrapCheckBox<TModel>> initEditor = null,
				Action<IBootstrapLabel> initLabel = null)
			{
				return bs
					.CheckBox(v)
						.Do(a => InitField(a.Class, a.HtmlAttributes, v))
						.Do(initEditor)
					.Label()
						.Do(a => InitLabel(a, v, initLabel));
			}

			#endregion


			#region TextArea

			public IHtmlString TextArea<TM>(FormBuilder<TM> f, ViewMember v,
				Action<BootstrapControlGroupTextArea<TM>> initEditor = null,
				Action<IBootstrapLabel> initLabel = null)
			{
				return f
					.FormGroup()
						.Do(g => InitFieldValidation(g, v))
					.TextArea(v)
						.Do(a => InitField(a.Class, a.HtmlAttributes, v))
						.Do(initEditor)
					.Label()
						.Do(a => InitLabel(a, v, initLabel));
			}

			#endregion


			#region TextBox

			public IHtmlString TextBox<TM>(FormBuilder<TM> f, ViewMember v,
				string type = null,
				string suggest = null,
				Action<BootstrapControlGroupTextBox<TM>> initEditor = null,
				Action<IBootstrapLabel> initLabel = null)
			{
				return f
					.FormGroup()
						.Do(g => InitFieldValidation(g, v))
					.TextBox(v)
						.Do(a => a.Placeholder(v.title.Short))
						.Do(a => InitTextBox(a.Class, a.HtmlAttributes, v, type, suggest))
						.Do(initEditor)
					.Label()
						.Do(a => InitLabel(a, v, initLabel));
			}

			public IHtmlString TextBox(ViewMember v,
				string type = null,
				string suggest = null,
				Action<BootstrapTextBox<TModel>> initEditor = null)
			{
				return bs
					.TextBox(v)
						.Do(e => InitFieldValidation(e.HtmlAttributes, v))
						.Do(a=> a.Placeholder(v.title.Short))
						.Do(a => InitTextBox(a.Class, a.HtmlAttributes, v, type, suggest))
						.Do(initEditor);
			}


			void InitTextBox<T>(Func<string, T> setClass, Func<object, T> setAttrs,
				ViewMember v, string type, string suggest)
			{
				if (type == null)
				{
					switch (v.InputType)
					{
						case InputType.Email: type = "email"; break;
						case InputType.Phone: type = "phone"; break;
						case InputType.Url: type = "url"; break;
					}
				}

				if (type.Yes())
					setAttrs(new { type });

				suggest = suggest ?? v.ReferenceType.As(a => a.Name);
				if (suggest.Yes())
					setAttrs(new
					{
						typeahead = "a as a.Name for a in db." + suggest + ".suggest($viewValue)",
						//typeahead_editable = false,
					});

				InitField(setClass, setAttrs, v);
			}

			#endregion


			// ReSharper disable once UnusedParameter.Local
			void InitField<T>(Func<string, T> setClass, Func<object, T> setAttrs, ViewMember v)
			{
				setAttrs(new { ng_model = "r." + v });

				if (v.Required)
					setAttrs(new { required = true });
			}

			#endregion


			#region Validation

			public BootstrapControlGroupBase<TM> InitFieldValidation<TM>(BootstrapControlGroupBase<TM> g, ViewMember v)
			{
				return g.HtmlAttributes(new
				{
					ng_class = "{ 'has-error': form." + v + ".$dirty && !form." + v + ".$valid }",
					tooltip_html_unsafe = "{{fieldErrorAsText(form." + v + ".$error)}}",
					tooltip_trigger = "mouseenter"
				});
			}

			public T InitFieldValidation<T>(Func<object, T> setAttributes, ViewMember v)
			{
				return setAttributes(new
				{
					ng_class = "{ 'has-error': form." + v + ".$dirty && !form." + v + ".$valid }",
					tooltip_html_unsafe = "{{fieldErrorAsText(form." + v + ".$error)}}",
					tooltip_trigger = "mouseenter"
				});
			}

			#endregion

		}

	}

}
