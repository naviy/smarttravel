using System;
using System.Collections;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;

using Luxena.Travel.Services;




namespace Luxena.Travel
{



	//===g






	public abstract class ProductEditForm : EntityEditForm
	{

		//---g



		protected new static void RegisterEdit(string className, Type formType)
		{

			FormsRegistry.RegisterEdit(className, delegate(EditFormArgs args)
			{

				ConfigManager.GetEditConfig(args.Type,
					delegate(ItemConfig config)
					{
						AppService.GetDocumentOwners(
							delegate(object result)
							{
								ProductEditForm form = (ProductEditForm)Type.CreateInstance(formType);
								form.ProductOwners = (Reference[])result;
								form.Init(args, config);
								form.Open();
							}, null
						);
					})
				;

			});

		}



		//---g



		public virtual bool IsRefund { get { return false; } }


		public bool IsTicket
		{
			get { return _args.Type == ClassNames.AviaTicket; }
		}


		public bool IsMco
		{
			get { return _args.Type == ClassNames.AviaMco; }
		}


		public bool UseHandling
		{
			get { return AppManager.SystemConfiguration.UseAviaHandling && CanUseHandling(); }
		}


		public virtual bool CanUseHandling()
		{
			return true;
		}


		public bool DisplayOwnerAsLabel
		{
			// ReSharper disable once SuspiciousTypeConversion.Global
			get { return ProductOwners.Length == 1 && (InitData == null || Reference.Equals(((ProductDto)(object)InitData).Owner, ProductOwners[0])); }
		}


		public Reference[] ProductOwners;



		//---g



		protected override void PreInitialize()
		{
			base.PreInitialize();


			foreach (Reference owner in ProductOwners)
			{
				((Dictionary)(object)owner).Remove("__type");
			}


			Window.cls += " aviaDocument-edit";
			Window.width = 910;
			Form.labelWidth = 140;

		}



		protected override Dictionary GetInitData()
		{
			return new Dictionary(
				"IssueDate", Date.Today,
				"TaxRateOfProduct", 0,
				"TaxRateOfServiceFee", 0
			);
		}



		//---g

	}






	//===g



}