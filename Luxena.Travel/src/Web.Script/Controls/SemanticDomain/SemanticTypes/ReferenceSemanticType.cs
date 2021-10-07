using System;
using System.Collections;

using Ext.data;

using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Field = Ext.form.Field;


namespace Luxena.Travel
{

	public class ReferenceSemanticType : SemanticType
	{
		public override string GetString(SemanticMember sm, object value)
		{
			return BasicViewForm.Link((Reference)value);
		}

		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			string className = sm._getReferenceEntity(sm._domain)._className;

			cfg.renderer(new RenderDelegate(delegate(object value)
			{
				if (!Script.IsValue(value)) return "";

				object[] values = value as object[];

				if (values != null)
				{
					if (!Script.IsValue(values[Reference.TypePos]))
						values[Reference.TypePos] = className;

					return ObjectLink.RenderArray(values);
				}

				Reference info = (Reference)value;

				if (!Script.IsValue(info.Type))
					info.Type = className;

				return ObjectLink.RenderInfo(info);
			}));
		}

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			string className = sm._getReferenceEntity(sm._domain)._className;

			if (sm._maxLength != 0)
			{
				width = sm._maxLength * 6;
				if (width < ControlFactory.MinWidth)
					width = ControlFactory.MinWidth;
				else if (width > ControlFactory.MaxWidth)
					width = ControlFactory.MaxWidth;
			}
			else if (Script.IsNullOrUndefined(width))
				width = form.FieldMaxWidth;

			ObjectSelectorConfig cfg = (ObjectSelectorConfig)new ObjectSelectorConfig()
				.customActionsDelegate(GetGetCustomActions(sm))
				.allowCreate(false)
				.setClass(className)
				.selectOnFocus(true)
				.forceSelection(true);


			OperationPermissions status = ((OperationPermissions)AppManager.AllowedActions[className]);

			//if (status.CanCreate.IsEnabled)
			//	cfg.allowCreate(true);

			if (status.CanUpdate.IsEnabled)
				cfg.allowEdit(true);

			InitTextFieldConfig(form, sm, width, cfg);

			FormMember member = form.Members.Add2(form, sm, cfg, initMember);

			ObjectSelector selector = new ObjectSelector(cfg);

			member.OnSaveValue += delegate
			{
				form.SetValue(sm._name, selector.GetObjectInfo());
			};

			return member.SetField(selector.Widget);
		}


		private GetSelectorCustomActionsDelegate GetGetCustomActions(SemanticMember sm)
		{
			SemanticEntity refEntity = sm._getReferenceEntity(sm._domain);

			return delegate(Store store, Ext.data.Record[] records, string query)
			{
				if (string.IsNullOrEmpty(query)) return null;

				if (records != null)
				{
					foreach (Ext.data.Record r in records)
					{
						string text = (string)r.get(sm._nameFieldName);

						if (string.Equals(text, query, true))
							return null;
					}
				}

				ArrayList actions = new ArrayList();

				SemanticEntity[] entities = Script.IsValue(refEntity._getDerivedEntities)
					? refEntity._getDerivedEntities(sm._domain)
					: new SemanticEntity[] { refEntity };

				foreach (SemanticEntity entity in entities)
				{
					actions.Add(new Ext.Action(new Ext.ActionConfig()
						.text(Res.Add_Action_Title + " " + (entity._title ?? entity._className))
						.handler(GetAddActionHandler(sm, refEntity, entity._className))
						.ToDictionary())
					);
				}

				return (Ext.Action[])actions;
			};
		}

		private static ComboBoxCustomActionDelegate GetAddActionHandler(SemanticMember sm, SemanticEntity refEntity, string className)
		{
			return delegate(ObjectSelector selector, string text)
			{
				Dictionary newData = new Dictionary();
				newData[refEntity._nameFieldName] = text;

				if (Script.IsValue(refEntity))
					refEntity._prepareNewData(newData);

				sm.EditForm.Updating = true;

				FormsRegistry.EditObject(
					className, null, newData,
					delegate(object result)
					{
						sm.EditForm.Updating = false;
						selector.SetValue(refEntity.getReference(((ItemResponse)result).Item, className));
					},
					delegate
					{
						sm.EditForm.Updating = false;
					}, 
					null
				);
			};
		}
	}


	public static partial class ViewTypes
	{
		public static SemanticType Reference = new ReferenceSemanticType();
	}


	public delegate SemanticEntity GetReferenceEntityEvent(SemanticDomain dsm);

	public partial class SemanticMember
	{
		public bool _isReference;
		public Func<SemanticDomain, SemanticEntity> _getReferenceEntity;

		public SemanticMember Reference(string referenceClassName)
		{
			_type = ViewTypes.Reference;
			_isReference = true;
			// ReSharper disable once SuspiciousTypeConversion.Global
			_getReferenceEntity = delegate(SemanticDomain dsm) { return (SemanticEntity)((Dictionary)(object)dsm)[referenceClassName]; };
			return this;
		}
	}

}