using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luxena.Travel.Domain;


namespace Luxena
{

	public static class ProductEx
	{

		public static string GetTypeNameUA(ProductType type)
		{
			switch (type)
			{
				case ProductType.AviaTicket:
					return "Авіаквиток";
				case ProductType.AviaRefund:
					return "Повернення авіаквитка";
				case ProductType.AviaMco:
					return "MCO";
				case ProductType.Pasteboard:
					return "Залізничний квиток";
				case ProductType.SimCard:
					return "SIM-картка";
				case ProductType.Isic:
					return "Студентський квиток";
				case ProductType.Excursion:
					return "Екскурсія";
				case ProductType.Tour:
					return "Турпакет";
				case ProductType.Accommodation:
					return "Готель";
				case ProductType.Transfer:
					return "Трансфер";
				case ProductType.Insurance:
					return "Страховка";
				case ProductType.CarRental:
					return "Оренда автомобіля";
				case ProductType.GenericProduct:
					return "Додаткова послуга";
				case ProductType.BusTicket:
					return "Автобусний квиток";
				case ProductType.PasteboardRefund:
					return "Повернення залізничного квитка";
				case ProductType.InsuranceRefund:
					return "Повернення страховки";
				case ProductType.BusTicketRefund:
					return "Повернення автобусного квитка";
			}

			return null;
		}
	}

}
