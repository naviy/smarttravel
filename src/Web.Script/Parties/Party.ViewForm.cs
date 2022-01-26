using System;
using System.Collections;
using System.Collections.Generic;

using Ext;

using jQueryApi;

using KnockoutApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Services;

using LxnTravel.Parties;




namespace Luxena.Travel
{



	//===g






	public abstract class PartyViewForm : Tab
	{

		//---g



		protected PartyViewForm(string tabId, object id, string className)
			: base(
				new PanelConfig()
					.closable(true)
					.autoScroll(true)
					.title(BaseRes.Loading)
					.bodyCssClass("banded party " + className.ToLowerCase())
					.ToDictionary(),
				tabId
			)
		{

			_className = className;


			ConfigManager.GetViewConfig(className,
				delegate (ItemConfig config)
				{
					SetButtonStatus(_editButton, config.IsEditAllowed);
					SetButtonStatus(_copyButton, config.IsCreationAllowed);
					SetButtonStatus(_deleteButton, config.IsRemovingAllowed);

					GetParty(id);
				}
			);

		}



		protected override void initComponent()
		{

			_editButton = new Button(new ButtonConfig()
				.text(BaseRes.Edit_Lower)
				.handler(new AnonymousDelegate(EditInstance))
				.ToDictionary());

			_copyButton = new Button(new ButtonConfig()
				.text(BaseRes.Copy_Lower)
				.handler(new AnonymousDelegate(CopyInstance))
				.ToDictionary());

			_deleteButton = new Button(new ButtonConfig()
				.text(BaseRes.Remove_Lower)
				.handler(new AnonymousDelegate(RemoveInstance))
				.ToDictionary());

			tbar = new Toolbar(new object[] { _editButton, _copyButton, _deleteButton });


			base.initComponent();

		}



		protected override void OnActivate(bool isFirst)
		{

			if (!isFirst)
				GetParty(Dto.Id);

		}



		protected abstract PartyModel CreateModel();

		protected abstract void GetParty(object id);



		protected void SetParty(object result)
		{

			if (result == null)
				return;


			Dto = (PartyDto)result;

			setTitle(Dto.Name);


			if (Model == null)
			{
				Model = CreateModel();

				InitModel();

				Ko.Mapping.FromJs(Dto, null, Model);

				View = jQuery.FromElement(Party.Main.RenderTo(body.dom, Model));

				InitView();
			}
			else
			{
				Ko.Mapping.FromJs(Dto, null, Model);
			}

		}



		protected virtual void InitModel()
		{

			Model.Edit = EditInstance;
			Model.AddFile = UploadFile;
			Model.DownloadFile = DownloadFile;
			Model.DeleteFile = DeleteFile;

			Model.AddOrder = delegate
			{
				FormsRegistry.EditObject(
					ClassNames.Order, null,
					new Dictionary(
						"Customer", Reference.Create(null, Dto.Name, Dto.Id)
					),
					delegate { GetParty(Dto.Id); }, null
				);
			};


			Model.OpenAllProducts = delegate { OpenAllProducts(); };
			Model.OpenAllOrders = delegate { OpenAllOrders(); };
			Model.OpenAllInvoices = delegate { OpenAllInvoices(); };

		}



		protected virtual void InitView()
		{
			InitTabs();
		}



		private static void SetButtonStatus(Button button, OperationStatus status)
		{

			if (Script.IsNullOrUndefined(status))
				return;


			button.setVisible(status.Visible);
			button.setDisabled(status.IsDisabled);
			button.setTooltip(status.DisableInfo);

		}



		private void InitTabs()
		{

			jQueryObject tabsMenu = View.Find(".tabs-menu li");
			jQueryObject tabsContent = View.Find(".tabs-content > div");
			jQueryObject currentTab = tabsMenu.Eq(0);
			jQueryObject currentContent = tabsContent.Eq(0);

			currentTab.AddClass("active");
			currentContent.CSS("display", "block");


			tabsMenu.Click(delegate (jQueryEvent e)
			{

				if (e.CurrentTarget == currentTab.GetElement(0))
					return;


				currentTab.RemoveClass("active");
				currentContent.Hide();
				currentTab = jQuery.FromElement(e.CurrentTarget);
				currentContent = tabsContent.Eq(currentTab.Index());
				currentTab.AddClass("active");
				currentContent.Show();

			});
			
		}



		private void EditInstance()
		{

			FormsRegistry.EditObject(
				
				_className,
				Dto.Id, 
				null,

				delegate (object response)
				{
					SetParty(((ItemResponse)response).Item);
				},

				null

			);

		}



		private void CopyInstance()
		{

			FormsRegistry.EditObject(_className, Dto.Id, null,
				delegate (object response)
				{
					PartyDto dto = (PartyDto)((ItemResponse)response).Item;
					FormsRegistry.ViewObject(_className, dto.Id);
				},
				null, null, LoadMode.Remote, true
			);

		}




		private void RemoveInstance()
		{

			MessageBoxWrap.Confirm(

				BaseRes.Confirmation, 
				BaseRes.Delete_Confirmation,

				delegate (string button, string text)
				{

					if (button != "yes")
						return;


					GenericService.Delete(
						
						_className,
						new object[] { Dto.Id },
						null,

						delegate (object result)
						{
							DeleteOperationResponse response = (DeleteOperationResponse)result;

							if (response.Success)
							{
								Tabs.Close(this);

								MessageRegister.Info(ListCaption, BaseRes.Deleted + " " + Dto.Name);
							}
							else
							{
								OnDeleteFailed();
							}
						},

						delegate
						{
							OnDeleteFailed();
						}

					);

				}

			);

		}



		protected void AddDetail(string className, string masterField)
		{

			Reference party = Reference.Create(null, Dto.Name, Dto.Id);

			Dictionary dictionary = new Dictionary();
			dictionary[masterField] = party;


			FormsRegistry.EditObject(
				className, null,
				dictionary, delegate { GetParty(Dto.Id); }, null
			);

		}



		protected void OnDeleteFailed()
		{

			//ReplaceForm.Exec(_className, Dto.Id, this);
			ReplaceForm.Exec("Party", Dto.Id, this);

		}



		//---g



		private void UploadFile()
		{

			FileUploadForm uploadForm = new FileUploadForm(Dto.Id);

			uploadForm.Uploaded += delegate (FileDto fileDto)
			{
				FileModel file = Ko.Mapping.FromJs<FileModel>(fileDto);

				Model.Files.Unshift(file);
			};


			uploadForm.Open();

		}



		private void DownloadFile(FileModel file)
		{

			ReportLoader.Load(
				string.Format("files/party/{0}", file.FileName.GetValue()),
				new Dictionary("file", file.Id.GetValue())
			);

		}



		private void DeleteFile(FileModel file)
		{

			MessageBoxWrap.Confirm(
				
				Res.Confirmation, 
				string.Format(Res.PartyView_Confirm_File_Delete, file.FileName.GetValue()),

				delegate (string button, string text)
				{

					if (button != "yes")
						return;

					PartyService.DeleteFile(file.Id.GetValue(),
						delegate
						{
							Model.Files.Remove(file);

							MessageRegister.Info(DomainRes.Party_Files, string.Format(Res.File_Deleted, file.FileName.GetValue()));
						},
						null
					);
				}

			);

		}



		//---g



		protected void OpenAllProducts()
		{

			PropertyFilter filter = new PropertyFilter();
			filter.Property = "Customer";
			PropertyFilterCondition condition = new PropertyFilterCondition();
			condition.Operator = FilterOperator.Equals;
			condition.Value = Dto.Name;
			filter.Conditions = new PropertyFilterCondition[] { condition };

			RangeRequest request = new RangeRequest();
			request.Filters = (PropertyFilter[])new List<PropertyFilter>(filter);

			FormsRegistry.ListObjects(ClassNames.Product, request, false);

		}



		protected void OpenAllOrders()
		{

			PropertyFilter filter = new PropertyFilter();
			filter.Property = "Customer";
			PropertyFilterCondition condition = new PropertyFilterCondition();
			condition.Operator = FilterOperator.Equals;
			condition.Value = Dto.Name;
			filter.Conditions = new PropertyFilterCondition[] { condition };

			RangeRequest request = new RangeRequest();
			request.Filters = (PropertyFilter[])new List<PropertyFilter>(filter);

			FormsRegistry.ListObjects(ClassNames.Order, request, false);

		}



		protected void OpenAllInvoices()
		{

			PropertyFilter filter = new PropertyFilter();
			filter.Property = "Customer";
			PropertyFilterCondition condition = new PropertyFilterCondition();
			condition.Operator = FilterOperator.Equals;
			condition.Value = Dto.Name;
			filter.Conditions = new PropertyFilterCondition[] { condition };

			RangeRequest request = new RangeRequest();
			request.Filters = (PropertyFilter[])new List<PropertyFilter>(filter);


			FormsRegistry.ListObjects(ClassNames.Invoice, request, false);

		}



		//---g



		protected string ListCaption;

		protected PartyDto Dto;
		protected PartyModel Model;
		protected jQueryObject View;

		private readonly string _className;

		private Button _editButton;
		private Button _copyButton;
		private Button _deleteButton;



		//---g

	}






	//===g



}