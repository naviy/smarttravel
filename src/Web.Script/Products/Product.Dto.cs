using System;
using System.Collections;

using Luxena.Travel.Services;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;
using Luxena.Travel.Controls;


namespace Luxena.Travel
{

	partial class ProductDto
	{

		public static bool IsCanDelete(ProductDto r)
		{
			return !Script.IsValue(r.CanDelete) || (r.CanDelete.Visible && !r.CanDelete.IsDisabled);
		}

		public static bool IsCanUpdate(ProductDto r)
		{
			return !Script.IsValue(r.CanUpdate) || (r.CanUpdate.Visible && !r.CanUpdate.IsDisabled);
		}


		public static void TryAddToExistingOrder(Dictionary documents, bool separateServiceFee, string messageTitle, AnonymousDelegate onAdded)
		{
			if (documents.Count == 0) return;

			RangeRequest request = new RangeRequest();
			request.Sort = "IssueDate";
			request.Dir = "DESC";
			request.VisibleProperties = new string[] { "Number", "IssueDate", "Customer", "AssignedTo", "Total", "Note" };


			FormsRegistry.SelectObjects(ClassNames.Order, request, true,
				delegate (object arg1)
				{
					if (((object[])arg1).Length != 1) return;

					object order = ((object[])arg1)[0];

					object[] docIds = AutoListTabExt2.GetDictionaryKeys(documents);

					OrderService.GetOrdersByAviaDocuments(docIds,
						delegate (object result)
						{
							OrderItemDto[] addedDocuments = (OrderItemDto[])result;

							if (addedDocuments == null || addedDocuments.Length == 0)
							{
								AddToExistingOrder(documents, order, separateServiceFee, messageTitle, onAdded);
								return;
							}

							ConfirmDocumentsAddition(addedDocuments, documents, order, separateServiceFee, messageTitle, onAdded);
						}, null
					);

				}, null
			);
		}


		public static void ConfirmDocumentsAddition(OrderItemDto[] addedDocuments, Dictionary documents, object orderId, bool separateServiceFee, string messageTitle, AnonymousDelegate onAdded)
		{
			foreach (OrderItemDto link in addedDocuments)
				documents.Remove((string)link.Product.Id);

			if (documents.Count > 0)
				MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocument_AddToOrder_Title, Res.AviaDocument_CannotAddDocumentsToOrder_Msg,
					addedDocuments, Res.AviaDocument_ContinueAddToOrder_Msg,
					delegate
					{
						AddToExistingOrder(documents, orderId, separateServiceFee, messageTitle, onAdded);
					});
			else
				MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocument_AddToOrder_Title, Res.AviaDocument_CannotAddDocumentsToOrder_Msg, addedDocuments);
		}


		public static void AddToExistingOrder(Dictionary documents, object orderId, bool separateServiceFee, string messageTitle, AnonymousDelegate onAdded)
		{
			OrderService.AddAviaDocuments(orderId, AutoListTabExt2.GetDictionaryKeys(documents), separateServiceFee,
				delegate (object result)
				{
					ItemResponse response = (ItemResponse)result;
					if (response.Errors != null)
					{
						MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocument_AddToOrder_Title,
							Res.Order_CannotRestoreOrder_Msg, (OrderItemDto[])response.Errors);
					}
					else
					{
						string message = string.Format(Res.AviaDocument_Was_Added, AutoListTabExt2.GetDictionaryValues(documents).Join(", "), ObjectLink.RenderInfo((Reference)response.Item));
						MessageRegister.Info(messageTitle, message);

						onAdded();

						FormsRegistry.ViewObject(ClassNames.Order, ((Reference)response.Item).Id);
					}
				}, null
			);
		}


	}

}
