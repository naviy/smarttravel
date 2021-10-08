using System;
using System.Collections;

using Ext.data;
using Ext.form;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Services;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;
using ComboBox = LxnBase.UI.Controls.ComboBox;
using Record = Ext.data.Record;


namespace Luxena.Travel.Controls
{



	//===g






	public class ControlFactoryExt
	{

		//---g



		public static void Init()
		{
			ControlFactory.RegisterCustomRenderer("Money",
				delegate(object value, object metadata, Record record, int rowindex, int colindex, Store store)
				{
					if (Script.IsNullOrUndefined(value))
						return string.Empty;

					string moneyStr;

					object[] values = value as object[];

					if (values != null)
						moneyStr = string.Format("{0} {1}", ((decimal)values[0]).Format("N2"), values[1]);
					else
					{
						MoneyDto dto = (MoneyDto)value;

						moneyStr = string.Format("{0} {1}", dto.Amount.Format("N2"), dto.Currency.Name);
					}

					return string.Format("<div style='text-align: right'>{0}</div>", moneyStr);
				});

			ControlFactory.RegisterCustomEditor(ClassNames.Money,
				delegate(ColumnConfig config, bool listMode)
				{
					if (listMode)
						return null;

					return new MoneyControl(MoneyControlConfig.DefaultConfig(string.Empty, true));
				}
			);

		}



		//---g



		public static Delegate GetMoneyRenderer()
		{
			CustomTypeColumnConfig moneyConfig = new CustomTypeColumnConfig();
			moneyConfig.Type = TypeEnum.Custom;
			moneyConfig.TypeName = ClassNames.Money;

			return ControlFactory.CreateRenderer(moneyConfig);
		}



		public static ObjectSelectorConfig CreateCustomerConfig(string fieldName, int width, bool allowBlank)
		{
			return (ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				.allowEdit(true)
				.allowCreate(false)
				.customActionsDelegate(GetCustomerSelectorActions)
				.fieldLabel(fieldName)
				.width(width)
				.allowBlank(allowBlank)
			;
		}



		public static ObjectSelector CreateCustomerControl(string fieldName, int width, bool allowBlank)
		{
			return new ObjectSelector(CreateCustomerConfig(fieldName, width, allowBlank));
		}



		public static ObjectSelector CreateCustomerControlWithText(string fieldName, int width, bool allowBlank)
		{
			return new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				.allowEdit(true)
				.allowCreate(false)
				.customActionsDelegate(GetCustomerSelectorActions)
				.forceSelection(false)
				.fieldLabel(fieldName)
				.width(width)
				.allowBlank(allowBlank));
		}



		public static ObjectSelector CreateAssignedToControl(string fieldName, int width, bool allowBlank)
		{
			return new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Person)
				.setDataProxy(PartyService.SuggestUsersProxy())
				.allowEdit(false)
				.allowCreate(false)
				.fieldLabel(fieldName)
				.width(width)
				.allowBlank(allowBlank));
		}



		public static ComboBox CreateOwnerControl(int width)
		{

			ComboBox control = new ComboBox(new ComboBoxConfig()
				.name("Office")
				.store(new JsonStore(new JsonStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(AppManager.Departments)
					.ToDictionary()))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.fieldLabel(DomainRes.Common_Owner)
				.allowBlank(false)
				.ToDictionary())
			;


			if (!AppManager.AllowSetDocumentOwner)
			{
				control.setValue(AppManager.Departments[0]);
				control.disable();
			}


			return control;

		}



		public static ComboBox CreateBankAccountControl(int width)
		{
			return new ComboBox(new ComboBoxConfig()
				.name("Office")
				.store(new JsonStore(new JsonStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(AppManager.BankAccounts)
					.ToDictionary()))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.fieldLabel("Банковский счёт")
				.ToDictionary()
			);
		}



		private static Action[] GetCustomerSelectorActions(Store store, Record[] records, string query)
		{
			if (string.IsNullOrEmpty(query))
				return null;

			if (records != null)
				foreach (Record r in records)
				{
					string text = (string)r.get("Name");

					if (string.Equals(text, query, true))
						return null;
				}

			return new Action[]
			{
				new Action(new ActionConfig()
					.text(Res.AddPerson_Action_Title)
					.handler(new ComboBoxCustomActionDelegate(
						delegate(ObjectSelector selector1, string text1)
						{
							AddParty(selector1, text1, ClassNames.Person);
						}))
					.ToDictionary()),

				new Action(new ActionConfig()
					.text(Res.AddOrganization_Action_Title)
					.handler(new ComboBoxCustomActionDelegate(
						delegate(ObjectSelector selector2, string text2)
						{
							AddParty(selector2, text2, ClassNames.Organization);
						}))
					.ToDictionary())
			};
		}



		private static void AddParty(ObjectSelector selector, string text, string type)
		{

			Reference info = new Reference();


			if (AppManager.SystemConfiguration.IsOrganizationCodeRequired && type == ClassNames.Organization)
			{
				Dictionary nameDictionary = new Dictionary("Name", text, "IsCustomer", true);

				FormsRegistry.EditObject(type, null, nameDictionary,
					delegate(object result)
					{
						OrganizationDto dto = (OrganizationDto)((ItemResponse)result).Item;
						info.Id = dto.Id;
						info.Name = dto.Name;
						info.Type = ClassNames.Organization;

						selector.SetValue(info);
					}, null, null);

				return;
			}


			PartyService.CreateCustomer(type, text,
				delegate(object result)
				{
					Dictionary party = Dictionary.GetDictionary(result);

					info.Id = party[ObjectPropertyNames.Id];
					info.Name = (string)party[ObjectPropertyNames.Reference];
					info.Type = (string)party[ObjectPropertyNames.ObjectClass];

					selector.SetValue(info);

					string caption = info.Type == ClassNames.Person ? DomainRes.Person_Caption_List : DomainRes.Organization_Caption_List;

					MessageRegister.Info(caption, BaseRes.Created + " " + info.Name);
				},
				null
			);

		}



		//---g

	}






	//===g



}