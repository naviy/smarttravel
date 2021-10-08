using System;
using System.Collections;

using Ext;

using LxnBase;
using LxnBase.UI;

using Luxena.Travel.Services;

using Action = Ext.Action;


namespace Luxena.Travel.Configuration
{
	public class ProfileViewForm : Tab
	{
		public ProfileViewForm(string tabId, object id)
			: base(new PanelConfig()
				.closable(true)
				.autoScroll(true)
				.title(BaseRes.Loading)
				.cls("profile-view")
				.bodyStyle("padding: 20px")
				.ToDictionary(), tabId)
		{
			UserService.GetUserProfile(id, delegate(object result)
			{
				_profile = (ProfileDto) result;

				Initialize();
			},
			null);
		}

		protected override void initComponent()
		{
			Action changePasswordButton = new Action(new ActionConfig()
				.text(Res.Password_Edit)
				.handler(new AnonymousDelegate(ChangePassword))
				.ToDictionary());

			tbar = new Toolbar(new object[] { changePasswordButton });

			base.initComponent();
		}

		public static void ViewObject(object id, bool newTab)
		{
			Tabs.Open(newTab, "myProfileForm", delegate(string tabId) { return new ProfileViewForm(tabId, id); });
		}

		private void Initialize()
		{
			setTitle(Res.Profile_Item);

			InitializePersonPanel();
			InitializeGdsAgentsPanel();

			Panel pnl = new Panel(new PanelConfig()
				.items(new object[] { _personInfoPanel, _gdsAgentsPanel })
				.border(false)
				.ToDictionary());

			add(pnl);
			doLayout();
		}

		private void InitializeGdsAgentsPanel()
		{
			_gdsAgentsPanel = new Panel(
				new PanelConfig().border(false).cls("gds-panel").listeners(new Dictionary("render",
					new AnonymousDelegate(RenderGdsAgents))).
					ToDictionary());
		}

		private void InitializePersonPanel()
		{
			_personInfo = new Panel(new PanelConfig()
				.border(false)
				.listeners(new Dictionary("render", new AnonymousDelegate(LoadPersonInfo)))
				.ToDictionary());

			_personInfoPanel = new Panel(new PanelConfig()
				.items(new object[] { _personInfo })
				.border(false)
				.ToDictionary());
		}

		private void LoadPersonInfo()
		{
			StringBuilder template = new StringBuilder();
			template.Append("<div class='viewItem'>");
			template.Append(string.Format("<div class='itemCaption'>{0}:</div>", DomainRes.Person));
			template.Append(string.Format("<div class='itemValue'>{0}</div>", _profile.Person.Name));
			template.Append("</div>");
			template.Append("<div class='viewItem'>");
			template.Append(string.Format("<div class='itemCaption'>{0}:</div>", DomainRes.User_Name));
			template.Append(string.Format("<div class='itemValue'>{0}</div>", _profile.Login));
			template.Append("</div>");
			template.Append("<div class='viewItem'>");
			template.Append(string.Format("<div class='itemCaption'>{0}:</div>", DomainRes.User_Roles));
			template.Append(string.Format("<div class='itemValue'>{0}</div>", _profile.Roles));
			template.Append("</div>");
			new Template(template.ToString()).overwrite(_personInfo.body);
		}

		private void RenderGdsAgents()
		{
			StringBuilder table = new StringBuilder();

			if (_profile.GdsAgents != null && _profile.GdsAgents.Length > 0)
			{
				table.Append(
					string.Format(
						@"<div class='gdsAgentsCaption'>{0}</div>
							<table cellspacing='0' cellpadding='0'>
								<tr>
									<th style='width:200px'>{1}</th>
									<th style='width:85px'>{2}</th>
									<th style='width:115px'>{3}</th>
								</tr><tpl for=""GdsAgents"">
									<tr>
										<td align='right'>
											{Origin}
										</td>
										<td align='left'>
											{OfficeCode}
										</td>
										<td align='center'>
												{AgentCode}
										</td>
									</tr>
								</tpl>
							</table>",
						DomainRes.GdsAgent_Caption_List,
						DomainRes.GdsAgent_Origin, DomainRes.GdsAgent_Office, DomainRes.GdsAgent_Agent));
			}
			else table.Append(string.Format(@" <div class='noData'>{0}</div>", Res.GdsAgents_NotAssigned));

			new XTemplate(table.ToString()).overwrite(_gdsAgentsPanel.body, _profile);
		}

		private static void ChangePassword()
		{
			PasswordEditForm form = new PasswordEditForm();
			form.Open();
		}

		private ProfileDto _profile;
		private Panel _personInfoPanel;
		private Panel _personInfo;
		private Panel _gdsAgentsPanel;
	}
}