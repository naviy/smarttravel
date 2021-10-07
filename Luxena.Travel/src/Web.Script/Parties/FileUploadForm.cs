using System;
using System.Collections;
using System.Net;
using System.Serialization;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.Net;


namespace Luxena.Travel
{
	public class FileUploadForm
	{
		public FileUploadForm(object partyId)
		{
			_partyId = partyId;

			_uploadFormPanel = new FormPanel(new FormPanelConfig()
				.bodyBorder(false)
				.border(false)
				.items(new object[] {
					new BoxComponent(new BoxComponentConfig()
						.autoEl(new Dictionary(
								"tag", "input",
								"type", "file",
								"name", "uploadedFile",
								"size", 30
								))
						.ToDictionary()) })
				.ToDictionary());

			_uploadFormPanel.getForm().fileUpload = true;
			_uploadFormPanel.getForm().standardSubmit = false;
			_uploadFormPanel.getForm().timeout = 50000;
			_uploadFormPanel.getForm().waitTitle = Res.File_WaitUpload;
			
			_window = new Window(new WindowConfig()
				.title(Res.FileUpload_Title)
				.baseCls("x-panel")
				.cls("file-upload")
				.items(_uploadFormPanel)
				.resizable(false)
				.modal(true)
				.buttons(new object[]
					{
						new Button(new ButtonConfig()
							.text(Res.File_UploadAction)
							.handler(new AnonymousDelegate(UploadFile))
							.ToDictionary()),
						new Button(new ButtonConfig()
							.text(Res.Close_Action)
							.handler(new AnonymousDelegate(Close))
							.ToDictionary()),
					})
				.buttonAlign("center")
				.ToDictionary());
		}

		public event Action<FileDto> Uploaded;

		public void Open()
		{
			_window.addClass("file-form");
			_window.show();
		}
		
		private void UploadFile()
		{
			_uploadFormPanel.getForm().submit(
				new Dictionary(
					"url", WebService.Root + "PartyService.asmx/UploadFile",
					"params", new Dictionary("partyId", _partyId),
					"waitMsg", Res.File_WaitUpload,
					"success", new Action<FileUploadForm, Ext.form.Action>(FileUploaded),
					"failure", new Action<FileUploadForm, Ext.form.Action>(UploadFailed)));
		}

		private void Close()
		{
			_window.close();
		}

		private void FileUploaded(FileUploadForm form, Ext.form.Action action)
		{
			XmlHttpRequest request = (XmlHttpRequest) action.response;

			FileDto result = (Json.ParseData<UploadFileResponse>(request.ResponseText)).File;

			Close();

			MessageRegister.Info(DomainRes.Party_Files, string.Format(Res.File_Upload_Success, result.FileName));

			if (Uploaded != null)
				Uploaded(result);
		}

		private void UploadFailed(FileUploadForm form, Ext.form.Action action)
		{
			UploadFileResponse result = (UploadFileResponse) action.result;

			MessageRegister.Error(DomainRes.Party_Files, string.Format(Res.File_Upload_Failed, result.ErrorMessage));
		}

		private readonly Window _window;
		private readonly FormPanel _uploadFormPanel;

		private readonly object _partyId;
	}
}