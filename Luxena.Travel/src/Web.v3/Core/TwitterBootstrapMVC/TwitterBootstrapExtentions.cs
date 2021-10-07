using System;
using System.IO;
using System.Web;

using Luxena.Domain.Views;

using TwitterBootstrap3;

using TwitterBootstrapMVC;
using TwitterBootstrapMVC.BootstrapMethods;
using TwitterBootstrapMVC.Controls;


namespace Luxena.Domain.Web
{

	public static class TwitterBootstrapExtentions
	{

		#region Utilites

		public static TextWriter GetWriter<TModel>(this BootstrapBase<TModel> bs)
		{
			return bs.Html.ViewContext.Writer;
		}

		public static void Write<TModel>(this BootstrapBase<TModel> bs, string text)
		{
			var writer = bs.Html.ViewContext.Writer;
			writer.Write(text);
		}

		public static void WriteLine<TModel>(this BootstrapBase<TModel> bs, string text)
		{
			var writer = bs.Html.ViewContext.Writer;
			writer.WriteLine(text);
		}


		#endregion


		#region Html

		#region CssStyle

		public static T CssStyle<T>(this BootstrapButtonBase<T> me, string value)
			where T : BootstrapButtonBase<T>
		{
			return me.HtmlAttributes(new { style = value });
		}

		public static BootstrapLink<TModel> CssStyle<TModel>(this BootstrapLink<TModel> me, string value)
		{
			return me.HtmlAttributes(new { style = value });
		}

		public static BootstrapTextBox<T> CssStyle<T>(this BootstrapTextBox<T> me, string value)
		{
			return me.HtmlAttributes(new { style = value });
		}



		public static IHtmlString Col<TModel>(this BootstrapBase<TModel> bs,
			int? all = null,
			int? offsetSm = null, int? offsetMd = null, int? offsetLg = null,
			int? sm = null, int? md = null, int? lg = null,
			IHtmlString html = null)
		{
			string classes = null;

			if (offsetSm.HasValue)
				classes += " col-sm-offset-" + offsetSm;
			if (offsetMd.HasValue)
				classes += " col-md-offset-" + offsetMd;
			if (offsetLg.HasValue)
				classes += " col-lg-offset-" + offsetLg;

			if (sm.HasValue)
				classes += " col-sm-" + sm;
			else if (all.HasValue)
				classes += " col-sm-" + all;

			if (md.HasValue)
				classes += " col-md-" + md;
			else if (all.HasValue)
				classes += " col-md-" + all;

			if (lg.HasValue)
				classes += " col-lg-" + lg;
			else if (all.HasValue)
				classes += " col-lg-" + all;

			return classes == null ? html : new HtmlString(@"<div class=""" + classes + @""">" + html + "</div>");
		}

		#endregion


		#region Title

		public static T Title<T>(this BootstrapButtonBase<T> me, string value)
			where T : BootstrapButtonBase<T>
		{
			return me.HtmlAttributes(new { title = value });
		}

		public static BootstrapLink<TModel> Title<TModel>(this BootstrapLink<TModel> me, string value)
		{
			return me.HtmlAttributes(new { title = value });
		}

		#endregion

		#endregion


		#region Buttons

		public static object Buttons<TModel>(
			this BootstrapBase<TModel> bs,
			params Func<ButtonGroupBuilder<TModel>, IHtmlString>[] btns)
		{

			var writer = bs.GetWriter();

			using (var g = bs.Begin(new ButtonGroup()))
			{
				foreach (var btn in btns)
				{
					btn(g).Do(writer.WriteLine);
				}
			}

			return null;
		}


		public delegate Func<DropDownBuilder<TModel>, IHtmlString>[] BootstrapButtonItemsSetter<TModel>(params Func<DropDownBuilder<TModel>, IHtmlString>[] items);
		public delegate Func<DropDownBuilder<TModel>, IHtmlString>[] BootstrapButtonItemsGetter<TModel>(BootstrapButtonItemsSetter<TModel> items);

		public static BootstrapButton<TModel> IconButton<TModel>(
			this BootstrapBase<TModel> bs,
			ButtonGroupBuilder<TModel> g,
			string icon, string tooltip = null, string text = null,
			ButtonStyle style = ButtonStyle.Default,
			Action<Icon> initIcon = null,
			BootstrapButtonItemsGetter<TModel> items = null)
		{

			icon = CssIcon(icon);

			var items_ = items == null ? null : items(a => a);

			if (items_.No())
			{
				var btn = g != null ? g.Button() : bs.Button();
				btn.Text(text ?? "");

				if (icon.Yes())
				{
					btn.PrependIcon(new Icon(icon + " fa-lg btn-icon-p1").Do(initIcon));
				}

				// Fake
				//if (tooltip.Yes())
				//	btn.ngTooltip(tooltip);

				return btn;
			}

			var writer = bs.GetWriter();

			// Fake
			var txt = @"<i class=""" + icon;
			if (tooltip.Yes())
				txt += @" fa-lg btn-icon-p1"" tooltip=""" + tooltip + @""" tooltip-placement=""bottom"">";
			else
				txt += @""">";
			if (text.Yes())
				txt += "&nbsp;" + text;
			txt += "</i>";

			var ddBtn = new DropDown(txt).Style(style);
			using (var dd = g != null ? g.BeginDropDown(ddBtn) : bs.Begin(ddBtn))
			{
				// ReSharper disable once PossibleNullReferenceException
				foreach (var item in items_)
				{
					item(dd).Do(writer.WriteLine);
				}
			}

			return null;
		}

		public static BootstrapButton<TModel> IconButton<TModel>(
			this BootstrapBase<TModel> bs,
			string icon, string tooltip = null, string text = null,
			ButtonStyle style = ButtonStyle.Default,
			Action<Icon> initIcon = null,
			BootstrapButtonItemsGetter<TModel> items = null)
		{
			return bs.IconButton(null,
				icon, tooltip: tooltip, text: text,
				style: style,
				initIcon: initIcon,
				items: items
			);
		}


		public static BootstrapButton<TModel> DangerStyle<TModel>(this BootstrapButton<TModel> btn)
		{
			return btn.Style(ButtonStyle.Danger);
		}

		public static BootstrapButton<TModel> InfoStyle<TModel>(this BootstrapButton<TModel> btn)
		{
			return btn.Style(ButtonStyle.Info);
		}

		public static BootstrapButton<TModel> LinkStyle<TModel>(this BootstrapButton<TModel> btn)
		{
			return btn.Style(ButtonStyle.Link);
		}

		public static BootstrapButton<TModel> PrimaryStyle<TModel>(this BootstrapButton<TModel> btn)
		{
			return btn.Style(ButtonStyle.Primary);
		}

		public static BootstrapButton<TModel> SuccessStyle<TModel>(this BootstrapButton<TModel> btn)
		{
			return btn.Style(ButtonStyle.Success);
		}

		#endregion


		#region Links

		public static BootstrapLink<TModel> ngLink<TModel>(this DropDownBuilder<TModel> dd, string text, string ngClick)
		{
			return dd.Link(text, null).ngClick(ngClick);
		}

		#endregion


		#region Angular

		#region ngClass

		public static T ngClass<T>(this BootstrapButtonBase<T> me, string expression)
			where T : BootstrapButtonBase<T>
		{
			if (expression.No()) return (T)me;
			expression = expression.Replace("{", "{{").Replace("}", "}}");
			return me.HtmlAttributes(new { ng_class = expression });
		}

		public static BootstrapLink<TModel> ngClass<TModel>(this BootstrapLink<TModel> me, string expression)
		{
			if (expression.No()) return me;
			expression = expression.Replace("{", "{{").Replace("}", "}}");
			return me.HtmlAttributes(new { ng_class = expression });
		}

		public static BootstrapControlGroupBase<TModel> ngClass<TModel>(this BootstrapControlGroupBase<TModel> me, string expression)
		{
			if (expression.No()) return me;
			return me.HtmlAttributes(new { ng_class = expression });
		}

		public static Icon ngClass(this Icon me, string expression)
		{
			if (expression.No()) return me;
			expression = expression.Replace("{", "{{").Replace("}", "}}");
			return me.HtmlAttributes(new { ng_class = expression });
		}

		#endregion


		#region ngClick

		public static T ngClick<T>(this BootstrapButtonBase<T> me, string expression)
			where T : BootstrapButtonBase<T>
		{
			return me.Data(new { ng_click = expression });
		}

		public static BootstrapLink<TModel> ngClick<TModel>(this BootstrapLink<TModel> me, string expression)
		{
			return me.Data(new { ng_click = expression });
		}

		#endregion


		#region ngModel

		public static T ngModel<T>(this BootstrapButtonBase<T> me, string expression)
			where T : BootstrapButtonBase<T>
		{
			return me.Data(new { ng_model = expression });
		}

		public static TInput ngModel<TModel, TInput>(this BootstrapCheckBoxBase<TModel, TInput> me, string expression)
			where TInput : BootstrapCheckBoxBase<TModel, TInput>
		{
			return me.Data(new { ng_model = expression });
		}

		public static BootstrapLink<TModel> ngModel<TModel>(this BootstrapLink<TModel> me, string expression)
		{
			return me.Data(new { ng_model = expression });
		}

		public static TInput ngModel<TModel, TInput>(this BootstrapTextAreaBase<TModel, TInput> me, string expression)
			where TInput : BootstrapTextAreaBase<TModel, TInput>
		{
			return me.Data(new { ng_model = expression });
		}

		public static TInput ngModel<TModel, TInput>(this BootstrapTextBoxBase<TModel, TInput> me, string expression)
			where TInput : BootstrapTextBoxBase<TModel, TInput>
		{
			return me.Data(new { ng_model = expression });
		}

		#endregion


		#region ngDisabled

		public static T ngDisabled<T>(this BootstrapButtonBase<T> me, string expression)
			where T : BootstrapButtonBase<T>
		{
			return me.Data(new { ng_disabled = expression });
		}

		public static BootstrapLink<TModel> ngDisabled<TModel>(this BootstrapLink<TModel> me, string expression)
		{
			return me.Data(new { ng_disabled = expression });
		}

		#endregion


		#region ngHide

		public static T ngHide<T>(this BootstrapButtonBase<T> me, string expression)
			where T : BootstrapButtonBase<T>
		{
			return me.Data(new { ng_hide = expression });
		}

		public static BootstrapLink<TModel> ngHide<TModel>(this BootstrapLink<TModel> me, string expression)
		{
			return me.Data(new { ng_hide = expression });
		}

		#endregion


		#region ngTooltip

		public static T ngTooltip<T>(this BootstrapButtonBase<T> me, string text, string position = "bottom")
			where T : BootstrapButtonBase<T>
		{
			return me.Data(new { tooltip = text, tooltip_placement = position });
		}

		public static BootstrapLink<TModel> ngTooltip<TModel>(this BootstrapLink<TModel> me, string text, string position = "bottom")
		{
			return me.Data(new { tooltip = text, tooltip_placement = position });
		}

		#endregion

		#endregion


		#region Font Awesome

		public static string CssIcon(string icon)
		{
			return icon.No()
				? null
				: icon.StartsWith("icon-", "fa-", "fa fa-")
					? icon
					: "fa fa-" + icon;
		}

		public static string CssIcon(this ViewEntity me)
		{
			return CssIcon(me.As(a => a.icon));
		}

		#endregion

	}

}
