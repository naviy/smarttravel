using System;
using System.Collections;

using Ext.form;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Services;

using ComboBox = LxnBase.UI.Controls.ComboBox;


namespace Luxena.Travel
{

	public class LocalReferenceSemanticType : ReferenceSemanticType
	{

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			ComboBoxConfig config = new ReferenceSelectorConfig(null);

			InitTextFieldConfig(form, sm, width, config);

			FormMember member = form.Members.Add2(form, sm, config, initMember);

			ComboBox field = new ComboBox(config.ToDictionary());

			member.OnLoadValue += delegate
			{
				sm._localReferenceWebMethod(
					delegate(object data)
					{
						field.getStore().loadData(data);
						field.setValue(form.GetValue(sm._name));
					}
				);
			};

			member.OnSaveValue += delegate
			{
				form.SetValue(sm._name, field.GetObjectInfo());
			};

			return member.SetField(field);
		}


		public WebMethodDelegate GetSuggest(SemanticMember sm)
		{
			return delegate(AjaxCallback success)
			{
				if (!Script.IsValue(success)) return;

				RangeRequest prms = new RangeRequest();
				prms.Query = "*";
				prms.Limit = 500;

				string className = sm._getReferenceEntity(sm._domain)._className;

				GenericService.Suggest(className, prms, delegate(object data)
				{
					object[] list = (object[])((Dictionary)data)["List"];

					for (int i = 0; i < list.Length; i++)
					{
						list[i] = Reference.CreateFromArray(list[i]);
					}

					success(list);

				}, null);
			};
		}

	}


	public static partial class ViewTypes
	{
		public static LocalReferenceSemanticType LocalReference = new LocalReferenceSemanticType();
	}


	public delegate void WebMethodDelegate(AjaxCallback onSuccess);

	public partial class SemanticMember
	{
		public WebMethodDelegate _localReferenceWebMethod;

		public SemanticMember LocalReference(WebMethodDelegate webMethod)
		{
			_type = ViewTypes.LocalReference;
			_isReference = true;
			_localReferenceWebMethod = webMethod;
			return this;
		}

		public SemanticMember EnumReference(string referenceClassName)
		{
			_type = ViewTypes.LocalReference;
			_isReference = true;
			// ReSharper disable once SuspiciousTypeConversion.Global
			_getReferenceEntity = delegate(SemanticDomain dsm) { return (SemanticEntity)((Dictionary)(object)dsm)[referenceClassName]; };
			_localReferenceWebMethod = ViewTypes.LocalReference.GetSuggest(this);

			return this;
		}
	}

}