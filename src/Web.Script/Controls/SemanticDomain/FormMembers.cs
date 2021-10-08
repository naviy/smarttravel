using System.Collections;
using System.Collections.Generic;

using Ext.form;

using LxnBase;
using LxnBase.Services;
using LxnBase.UI;

using Luxena.Travel.Controls;


namespace Luxena.Travel
{

	public class FormMembers
	{
		public void Add(FormMember item)
		{
			Items.Add(item);
		}

		public FormMember Add2(IEditForm form, SemanticMember semantic, FieldConfig config, InitFormMemberAction initMember)
		{
			FormMember member = new FormMember(form, semantic, config);
			Items.Add(member);
			if (initMember != null)
				initMember(member);

			return member;
		}

		public FormMember Add3(IEditForm form, AnonymousDelegate onLoadValue, AnonymousDelegate onSaveValue)
		{
			FormMember member = new FormMember(form, null, null);
			member.OnLoadValue += onLoadValue;
			member.OnSaveValue += onSaveValue;

			Items.Add(member);

			return member;
		}

		public void AddToFieldList(ArrayList list)
		{
			foreach (FormMember item in Items) 
			{
				list.AddRange(item.GetFields());
			}
		}

		public void LoadValues()
		{
			foreach (FormMember item in Items) 
			{
				item.LoadValue();
			}
		}

		public void SaveValues()
		{
			foreach (FormMember item in Items) 
			{
				item.SaveValue();
			}
		}

		public bool IsModified()
		{
			foreach (FormMember item in Items) 
			{
				if (item.IsModified()) return true;
			}

			return false;
		}

		public readonly List<FormMember> Items = new List<FormMember>();
	}


	public interface IEditForm
	{
		EditFormArgs Args { get; }

		FormMembers Members { get; }
	
		int FieldMaxWidth { get; }
		
		bool Updating { get; set; }

		Dictionary Data { get; }

		object GetValue(string name);

		object GetCurrentValue(string name);

		void SetValue(string name, object value);
	}

	public interface IGridForm
	{
		Ext.grid.ColumnConfig ColumnCfg_(string name, bool isHidden, object elWidth, object renderer, GridColumnConfigAction initAction);

		ColumnConfig GetColumnConfigByName(string name);
	}

}