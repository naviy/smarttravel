using System;
using System.Runtime.CompilerServices;

using Ext;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.Controls;


namespace Luxena.Travel
{
	public static class MessageFactory
	{
		public static void Error(string msg)
		{
			MessageBoxWrap.Show(BaseRes.Error, msg, MessageBox.ERROR, MessageBox.OK);
		}

		public static void Warn(string msg)
		{
			MessageBoxWrap.Show(BaseRes.Warning, msg, MessageBox.WARNING, MessageBox.OK);
		}

		public static void ActionNotPermittedMsg()
		{
			MessageBoxWrap.Show(BaseRes.Error, BaseRes.AutoGrid_ActionNotPermitted_Msg, MessageBox.ERROR,MessageBox.OK);
		}

		public static void VoidingConfirmation(string voidMsg1, string voidMsg2, string voidMsg3,
			string restoreMsg1, string restoreMsg2, string restoreMsg3, bool isVoid, int count, AnonymousDelegate onConfirm)
		{
			MessageBoxWrap.Confirm(BaseRes.Confirmation, GetVoidingConfirmationText(voidMsg1, voidMsg2, voidMsg3,
				restoreMsg1, restoreMsg2, restoreMsg3, isVoid, count),
				delegate(string button, string text)
				{
					if (button != "yes")
							return;

					onConfirm();
				});
		}

		public static void VoidCompletedMsg(string title, string voidMsg1, string voidMsg2, string restoreMsg1, string restoreMsg2, bool isVoid, int count, string objectDisplayString, string details)
		{
			string message = isVoid
				? (count == 1 ? string.Format(voidMsg1, objectDisplayString) : string.Format(voidMsg2, count))
				: (count == 1 ? string.Format(restoreMsg1, objectDisplayString) : string.Format(restoreMsg2, count));

			MessageRegister.Info(title, message, details);
		}

		public static void ObjectUpdatedMsg(string caption, string objectDisplayString, bool isCreated)
		{
			MessageRegister.Info(caption, (isCreated ? BaseRes.Created : BaseRes.Updated) + " " + objectDisplayString);
		}

		public static void ContinueOperationConfirmation(string title, string errorMsg, string[] invalidItems, string continueOperationMsg, AnonymousDelegate onConfirm)
		{
			string newLine = "<br/>";

			StringBuilder builder = new StringBuilder();

			builder.Append(errorMsg);
			builder.Append(newLine);

			for (int i = 0; i < invalidItems.Length; i++)
			{
				string item = invalidItems[i];

				builder.Append(newLine);
				builder.Append(item);
			}

			if (string.IsNullOrEmpty(continueOperationMsg))
				MessageBoxWrap.Show(title, builder.ToString(), MessageBox.WARNING, MessageBox.OK, null);
			else
			{
				builder.Append(newLine);
				builder.Append(newLine);

				builder.Append(continueOperationMsg);

				MessageBoxWrap.Confirm(title, builder.ToString(),
					delegate(string button, string text)
					{
						if (button == "yes")
							onConfirm();
					});
			}
		}

		public static void InvoiceAddedToPayment(string title, Reference payment, Reference invoice)
		{
			string text = string.Format("{0} {1} {2}", DomainRes.Order.ToLowerCase(), invoice.Name, Res.Payment_AddedToPayment_Msg1);
			MessageRegister.Info(title, string.Format("{0} {1}", text, ObjectLink.RenderInfo(payment)));
		}

		[AlternateSignature]
		public static extern void DocumentsAlreadyAddedToOrder(string title, string msg, OrderItemDto[] sources);

		public static void DocumentsAlreadyAddedToOrder(string title, string msg, OrderItemDto[] sources, string confirmation, AnonymousDelegate onConfirm)
		{
			string separator = "<br/>";

			StringBuilder builder = new StringBuilder(msg);
			builder.Append(separator);
			builder.Append(separator);

			foreach (OrderItemDto link in sources)
			{
				builder.Append(string.Format(Res.Order_DocumentAlreadyAddedToOrder_Msg, link.Product.Name, link.Order.Name));
				builder.Append(separator);
			}

			if (!Script.IsNullOrUndefined(confirmation) && !Script.IsNullOrUndefined(onConfirm))
			{
				builder.Append(separator);
				builder.Append(confirmation);

				MessageBoxWrap.Confirm(title, builder.ToString(),
					delegate(string button, string text)
					{
						if (button != "yes")
							return;

						onConfirm();
					});
			}
			else
				MessageBoxWrap.Show(title, builder.ToString(), MessageBox.WARNING, MessageBox.OK);
		}

		public static void PostedPaymentsInInvoice(string title, string msg, PaymentDto[] payments)
		{
			string separator = "<br/>";

			StringBuilder builder = new StringBuilder(msg);
			builder.Append(separator);
			builder.Append(separator);

			foreach (PaymentDto payment in payments)
			{
				builder.Append(string.Format(Res.Order_PostedPaymentInOrder_Msg, payment.Number, payment.Order.Name));
				builder.Append(separator);
			}
			
			MessageBoxWrap.Show(title, builder.ToString(), MessageBox.WARNING, MessageBox.OK);
		}

		private static string GetVoidingConfirmationText(string voidMsg1, string voidMsg2, string voidMsg3,
			string restoreMsg1, string restoreMsg2, string restoreMsg3, bool isVoid, int count)
		{
			string msg1 = isVoid ? restoreMsg1 : voidMsg1;
			string msg2 = isVoid ? restoreMsg2 : voidMsg2;
			string msg3 = isVoid ? restoreMsg3 : voidMsg3;

			return StringUtility.GetNumberText(count, msg1, msg2, msg3);
		}
	}
}