namespace Luxena.Travel.Domain
{

	public static class Texts
	{

		public class Loc : LocalizationAttribute
		{
			public Loc(string en = null, string ens = null, string enDesc = null, string enShort = null, string enEmpty = null, string ru = null, string rus = null, string ru2 = null, string ru5 = null, string ruDesc = null, string ruShort = null, string ruEmpty = null, string ua = null, string uas = null, string ua2 = null, string ua5 = null, string uaDesc = null, string uaShort = null, string uaEmpty = null)
				: base(en: en, ens: ens, enDesc: enDesc, enShort: enShort, enEmpty: enEmpty, ru: ru, rus: rus, ru2: ru2, ru5: ru5, ruDesc: ruDesc, ruShort: ruShort, ruEmpty: ruEmpty, ua: ua, uas: uas, ua2: ua2, ua5: ua5, uaDesc: uaDesc, uaShort: uaShort, uaEmpty: uaEmpty)
			{
			}
		}


		public static Loc Address = new Loc(ru: "Адрес", ua: "Адреса");
		public static Loc Contacts = new Loc(ru: "Контакты");
		public static Loc DepartureDate = new Loc(ru: "дата отправления", ua: "дата відправлення");
		public static Loc Fax = new Loc(ru: "факс", ua: "факс");
		public static Loc From = new Loc(ru: "от", ua: "від");
		public static Loc Hotel = new Loc(ru: "отель", ua: "готель");
		public static Loc Itinerary = new Loc(ru: "маршрут", ua: "маршрут");
		public static Loc Number_Short = new Loc(en: "#", ru: "№", ua: "№");
		public static Loc Phone = new Loc(ru: "тел", ua: "тел");
		public static Loc ServiceFee = new Loc(ru: "Сервисный сбор", ua: "Сервісний збір");
		public static Loc SeatNumber = new Loc(ru: "место", ua: "місце");
		public static Loc Train = new Loc(ru: "поезд", ua: "поїзд");
		public static Loc Wagon = new Loc(ru: "вагон", ua: "вагон");
	}

}
