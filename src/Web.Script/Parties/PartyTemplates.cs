using System;


namespace LxnTravel
{
	public class PartyTemplates
	{
		static PartyTemplates()
		{
			/*jQueryTemplating.CreateTemplate("party.contacts", @"
				{{if Phone1 || Phone2}}
				<p class='phone'>${Phone1}{{if Phone2}}<br/>${Phone2}{{/if}}</p>
				{{/if}}
				{{if Fax}}<p class='fax'>${Fax}</p>{{/if}}
				{{if Email1 || Email2}}
				<p class='email'><a href='mailto:${Email1}' target='_blank'>${Email1}</a>{{if Email2}}<br/><a href='mailto:${Email2}' target='_blank'>${Email2}</a>{{/if}}</p>
				{{/if}}
				{{if WebAddress}}
				<p class='http'><a href='${WebAddressHref}' target='_blank'>${WebAddress}</a></p>
				{{/if}}
				{{if ActualAddress}}
				<div class='address'>
					<div class='label'>${LxnTravel.DomainRes.party_ActualAddress}</div>
					${ActualAddress}&nbsp;
					<span class='links'>
						<a class='yandex' href='http://maps.yandex.ru/?text=${encodeURI(ActualAddress)}' target='_blank' title='Yandex Maps'></a>
						<a class='google' href='http://maps.google.com/maps?q=${encodeURI(ActualAddress)}' target='_blank' title='Google Maps'></a>
					</span>
				</div>
				{{/if}}
				{{if LegalAddress}}
				<div class='address'>
					<div class='label'>${LxnTravel.DomainRes.party_LegalAddress}</div>
					${LegalAddress}&nbsp;
					<span class='links'>
						<a class='yandex' href='http://maps.yandex.ru/?text=${encodeURI(LegalAddress)}' target='_blank' title='Yandex Maps'></a>
						<a class='google' href='http://maps.google.com/maps?q=${encodeURI(LegalAddress)}' target='_blank' title='Google Maps'></a>
					</span>
				</div>
				{{/if}}");

			jQueryTemplating.CreateTemplate("party.list.detail", @"
				{{if Phone1 || Email1}}<div class='detail'>{{if Phone1}}<span>${Phone1}</span>{{/if}} {{if Email1}}<a href='mailto:${Email1}' target='_blank'>${Email1}</a>{{/if}}</div>{{/if}}");

			DepartmentList = jQueryTemplating.CreateTemplate("organization.department.list", @"
				{{if Departments}}
				{{each Departments}}
				<div class='department'>
					{{html LxnBase.UI.Controls.ObjectLink.render($value.Id, $value.Name, 'Department')}}
					{{tmpl($value) 'party.list.detail'}}
				</div>
				{{/each}}
				{{/if}}");

			EmployeeList = jQueryTemplating.CreateTemplate("organization.employee.list", @"
				{{if Employees}}
				{{each Employees}}
				<div class='employee'>
					{{html LxnBase.UI.Controls.ObjectLink.render($value.Id, $value.Name, 'Person')}}{{if $value.Title}}<span class='title'> - ${$value.Title}</span>{{/if}}
					{{tmpl($value) 'party.list.detail'}}
				</div>
				{{/each}}
				{{/if}}");

			View = jQueryTemplating.CreateTemplate(@"
				<div class='band-wrap'>
					<table class='band-table'>
						<tr>
							<td class='band-left'>
								<div id='header'>
									<h1 id='name'>${Name}</h1>
									{{if LegalName}}<div id='legalname'>${LegalName}</div>{{/if}}
									{{if IsCustomer || IsSupplier}}
									<div id='roles'>
										{{if IsCustomer}}<span>${LxnTravel.Res.partyView_Role_Customer}</span>{{/if}}
										{{if IsSupplier}}<span>${LxnTravel.Res.partyView_Role_Supplier}</span>{{/if}}
									</div>
									{{/if}}
								</div>

								{{if HasEmployment}}
								<div id='employment'>
									{{if Title && Organization}}
									${Title}, {{html LxnBase.UI.Controls.ObjectLink.renderInfo(Organization)}}
									{{else Title}}
									${Title}
									{{else}}
									${LxnTravel.Res.personView_Employee} {{html LxnBase.UI.Controls.ObjectLink.renderInfo(Organization)}}
									{{/if}}
								</div>
								{{else Organization}}
									${LxnTravel.DomainRes.department} {{html LxnBase.UI.Controls.ObjectLink.renderInfo(Organization)}}
								{{/if}}
								{{if ReportsTo}}
								<div id='reportsto'>
									${LxnTravel.DomainRes.party_ReportsTo} {{html LxnBase.UI.Controls.ObjectLink.renderInfo(ReportsTo)}}
								</div>
								{{/if}}

								{{if Note}}
								<div id='note'>
									${Note}
								</div>
								{{/if}}

								<ul id='tabs-menu'><li>${LxnTravel.DomainRes.party_Files} (<span id='file-count'>${Files ? Files.length : 0}</span>)</li>{{if IsOrganization}}<li>${LxnTravel.DomainRes.organization_Employees} (<span id='employee-count'>${Employees ? Employees.length : 0}</span>)</li><li>${LxnTravel.DomainRes.department_Caption_List} (<span id='department-count'>${Departments ? Departments.length : 0}</span>)</li>{{/if}}</ul>

								<div id='tabs-content'>
									<div id='files'>
										<div class='actions'>
											<a id='add-file-action' href='javascript:void(0)'>${LxnTravel.Res.partyView_AddFile}</a>
										</div>
										{{each Files}}
										<div class='file'><a href='javascript:void(0)' class='get-action'>${$value.FileName}</a> <a href='javascript:void(0)' class='delete-action'></a><div>${$value.TimeStamp.format('d.m.Y H:i')} ${$value.UploadedBy.Text}</div></div>
										{{/each}}
									</div>
									{{if IsOrganization}}
									<div id='employees' class='list'>
										<div class='actions'>
											<a id='add-employee-action' href='javascript:void(0)'>${LxnTravel.Res.organizationView_AddEmployee}</a>
										</div>
										<div id='employee-list'>
										{{tmpl 'organization.employee.list'}}
										</div>
									</div>
									<div id='departments' class='list'>
										<div class='actions'>
											<a id='add-department-action' href='javascript:void(0)'>${LxnTravel.Res.organizationView_AddDepartment}</a>
										</div>
										<div id='department-list'>
										{{tmpl 'organization.department.list'}}
										</div>
									</div>
									{{/if}}
								</div>
							</td>
							<td class='band-right'>
								<div class='block-header'>${InfoTitle}</div>
								<div id='info'>
									{{tmpl 'party.contacts'}}

									{{if IsPerson}}
									{{if Birthday}}
									<p class='birthday'>${Birthday.format('d.m.Y')}</p>
									{{/if}}

									{{if MilesCards && MilesCards.length}}
									<div id='miles'>
										{{each MilesCards}}
										<div><span>${$value.Number}</span>{{if $value.Organization}}<br/><span>${$value.Organization.Text}</span>{{/if}}</div>
										{{/each}}
									</div>
									{{/if}}

									{{if Passports && Passports.length}}
									<div id='passports'>
										{{each Passports}}
										<div>
											<div class='pass-number'>{{if $value.IssuedByCode}}${$value.IssuedByCode}{{/if}} ${$value.Number}</div>
											{{if $value.ExpiredOn}}<div class='pass-expiredon'>${LxnTravel.Res.personView_Passport_ExpiredOn} ${$value.ExpiredOn.format('d.m.Y')}</div>{{/if}}
											{{if $value.Note}}<div class='pass-note'>${$value.Note}</div>{{/if}}
											<div class='pass-details'>
												<div>${$value.FirstName} ${$value.MiddleName} ${$value.LastName}</div>
												{{if $value.Birthday}}<div>${LxnTravel.DomainRes.passport_Birthday}: ${$value.Birthday.format('d.m.Y')}</div>{{/if}}
												{{if $value.GenderString}}<div>${LxnTravel.DomainRes.passport_Gender}: ${$value.GenderString}</div>{{/if}}
												{{if $value.Citizenship}}<div>${LxnTravel.DomainRes.passport_Citizenship}: ${$value.Citizenship.Text}</div>{{/if}}
											</div>
											<div class='pass-copy'>
												<a href='javascript:void(0)'>Amadeus</a><input readonly='readonly' value='${$value.AmadeusString}' />
												<a href='javascript:void(0)'>Galileo</a><input readonly='readonly' value='${$value.GalileoString}' />
											</div>
										</div>
										{{/each}}
									</div>
									{{/if}}
									{{else IsOrganization}}
									{{if Code}}
									<div id='code'>
										<div class='label'>${LxnTravel.DomainRes.organization_Code}</div>
										${Code}
									</div>
									{{/if}}
									{{/if}}
								</div>

								{{if Organization}}
								<div id='company' class='block-header'>${LxnTravel.Res.organizationView_Info}</div>
									{{html LxnBase.UI.Controls.ObjectLink.renderInfo(Organization)}}
									{{tmpl(OrganizationDto) 'party.contacts'}}
								{{/if}}
							</td>
						</tr>
					</table>
				</div>");*/
		}

		/*public static void AddWebAddressHref(PartyDto dto)
		{
			PartyViewModel viewModel = (PartyViewModel) (object) dto;

			if (!string.IsNullOrEmpty(dto.WebAddress))
				viewModel.WebAddressHref = _protocolCheck.Test(dto.WebAddress) ? dto.WebAddress : "http://" + dto.WebAddress;
		}*/

		private static readonly RegularExpression _protocolCheck = new RegularExpression("^.+://");
	}
}