using System;
using System.Runtime.CompilerServices;

using jQueryApi;

using KnockoutApi;

using LxnBase.Data;


namespace Luxena.Travel
{

	public class PartyModel
	{
		[PreserveCase]
		public Observable<object> Id = Ko.Observable<object>();

		[PreserveCase]
		public Observable<int> Version = Ko.Observable<int>();

		[PreserveCase]
		public Observable<string> Name = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> LegalName = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> Phone1 = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> Phone2 = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> Fax = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> Email1 = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> Email2 = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> WebAddress = Ko.Observable<string>();

		[PreserveCase]
		public Observable<bool> IsCustomer = Ko.Observable<bool>();

		[PreserveCase]
		public Observable<bool> IsSupplier = Ko.Observable<bool>();

		[PreserveCase]
		public Observable<Reference> ReportsTo = Ko.Observable<Reference>();

		[PreserveCase]
		public Observable<string> LegalAddress = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> ActualAddress = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> Note = Ko.Observable<string>();

		[PreserveCase]
		public Observable<PartyBalance> Balance = Ko.Observable<PartyBalance>();

		[PreserveCase]
		public ObservableArray<FileModel> Files = Ko.ObservableArray<FileModel>();

		public ObservableArray<OrderDto> Orders = Ko.ObservableArray<OrderDto>();

		public ObservableArray<InvoiceDto> Invoices = Ko.ObservableArray<InvoiceDto>();

		[PreserveCase]
		public Observable<OperationPermissions> Permissions = Ko.Observable<OperationPermissions>();

		[PreserveCase]
		public bool IsPerson;

		[PreserveCase]
		public bool IsOrganization;

		[PreserveCase]
		public bool IsDepartment;

		[PreserveCase]
		public string InfoTitle;

		[PreserveCase]
		public DependentObservable<bool> InfoPrompt;

		[PreserveCase]
		public Action Edit;

		[PreserveCase]
		public Action AddFile;

		[PreserveCase]
		public Action<FileModel> DownloadFile;

		[PreserveCase]
		public Action<FileModel> DeleteFile;

		[PreserveCase]
		public Action<jQueryEvent> AddOrder;

		[PreserveCase]
		public Action<jQueryEvent> OpenAllProducts;

		[PreserveCase]
		public Action<jQueryEvent> OpenAllOrders;

		[PreserveCase]
		public Action<jQueryEvent> OpenAllInvoices;

	}

}