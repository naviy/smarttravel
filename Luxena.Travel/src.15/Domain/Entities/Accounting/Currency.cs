//using System.Collections.Generic;


//namespace Luxena.Travel.Domain
//{

//	public class Currency
//	{

//		private Currency(string code, int numericCode, string cyrillicCode, string name)
//		{
//			Id = code;
//			Code = code;
//			NumericCode = numericCode;
//			CyrillicCode = cyrillicCode;
//			Name = name;
//		}


//		public string Id { get; set; }

//		public string Code { get; set; }

//		public int NumericCode { get; set; }

//		public string CyrillicCode { get; set; }

//		public string Name { get; set; }


//		public override string ToString()
//		{
//			return Code;
//		}

//		public static implicit operator Currency(string code)
//		{
//			return By(code);
//		}

//		public static implicit operator string(Currency currency)
//		{
//			return currency?.Code;
//		}

//		public static implicit operator int?(Currency currency)
//		{
//			return currency?.NumericCode;
//		}


//		public static Currency By(string codeOrId)
//		{
//			return codeOrId == null ? null : CurrenciesById.By(codeOrId);
//		}




//		public static readonly IDictionary<string, Currency> CurrenciesById = new SortedDictionary<string, Currency>
//		{
//			{ "AED", new Currency("AED", 784, null, "UAE Dirham") },
//			{ "AFN", new Currency("AFN", 971, null, "Afghani") },
//			{ "ALL", new Currency("ALL", 8, null, "Lek") },
//			{ "AMD", new Currency("AMD", 51, null, "Armenian Dram") },
//			{ "ANG", new Currency("ANG", 532, null, "Netherlands Antillian Guilder") },
//			{ "AOA", new Currency("AOA", 973, null, "Kwanza") },
//			{ "ARS", new Currency("ARS", 32, null, "Argentine Peso") },
//			{ "AUD", new Currency("AUD", 36, null, "Australian Dollar") },
//			{ "AUH", new Currency("AUH", -2, null, null) },
//			{ "AWG", new Currency("AWG", 533, null, "Aruban Guilder") },
//			{ "AZN", new Currency("AZN", 944, null, "Azerbaijanian Manat") },
//			{ "BAM", new Currency("BAM", 977, null, "Convertible Marks") },
//			{ "BBD", new Currency("BBD", 52, null, "Barbados Dollar") },
//			{ "BDT", new Currency("BDT", 50, null, "Taka") },
//			{ "BGN", new Currency("BGN", 975, null, "Bulgarian Lev") },
//			{ "BHD", new Currency("BHD", 48, null, "Bahraini Dinar") },
//			{ "BIF", new Currency("BIF", 108, null, "Burundi Franc") },
//			{ "BMD", new Currency("BMD", 60, null, "Bermudian Dollar") },
//			{ "BND", new Currency("BND", 96, null, "Brunei Dollar") },
//			{ "BOB", new Currency("BOB", 68, null, "Boliviano") },
//			{ "BOV", new Currency("BOV", 984, null, "Mvdol") },
//			{ "BRL", new Currency("BRL", 986, null, "Brazilian Real") },
//			{ "BSD", new Currency("BSD", 44, null, "Bahamian Dollar") },
//			{ "BTN", new Currency("BTN", 64, null, "Ngultrum") },
//			{ "BWP", new Currency("BWP", 72, null, "Pula") },
//			{ "BYR", new Currency("BYR", 974, null, "Belarussian Ruble") },
//			{ "BZD", new Currency("BZD", 84, null, "Belize Dollar") },
//			{ "CAD", new Currency("CAD", 124, null, "Canadian Dollar") },
//			{ "CDF", new Currency("CDF", 976, null, "Franc Congolais") },
//			{ "CHE", new Currency("CHE", 947, null, "WIR Euro") },
//			{ "CHF", new Currency("CHF", 756, null, "Swiss Franc") },
//			{ "CHW", new Currency("CHW", 948, null, "WIR Franc") },
//			{ "CLF", new Currency("CLF", 990, null, "Unidades de fomento") },
//			{ "CLP", new Currency("CLP", 152, null, "Chilean Peso") },
//			{ "CNY", new Currency("CNY", 156, null, "Yuan Renminbi") },
//			{ "COP", new Currency("COP", 170, null, "Colombian Peso") },
//			{ "COU", new Currency("COU", 970, null, "Unidad de Valor real") },
//			{ "CRC", new Currency("CRC", 188, null, "Costa Rican Colon") },
//			{ "CUC", new Currency("CUC", 931, null, "Peso Convertible") },
//			{ "CUP", new Currency("CUP", 192, null, "Cuban Peso") },
//			{ "CVE", new Currency("CVE", 132, null, "Cape Verde Escudo") },
//			{ "CZK", new Currency("CZK", 203, null, "Czech Koruna") },
//			{ "DJF", new Currency("DJF", 262, null, "Djibouti Franc") },
//			{ "DKK", new Currency("DKK", 208, null, "Danish Krone") },
//			{ "DOP", new Currency("DOP", 214, null, "Dominican Peso") },
//			{ "DZD", new Currency("DZD", 12, null, "Algerian Dinar") },
//			{ "EEK", new Currency("EEK", 233, null, "Kroon") },
//			{ "EGP", new Currency("EGP", 818, null, "Egyptian Pound") },
//			{ "ERN", new Currency("ERN", 232, null, "Nakfa") },
//			{ "ETB", new Currency("ETB", 230, null, "Ethiopian Birr") },
//			{ "EUR", new Currency("EUR", 978, null, "Euro") },
//			{ "FJD", new Currency("FJD", 242, null, "Fiji Dollar") },
//			{ "FKP", new Currency("FKP", 238, null, "Falkland Island Pound") },
//			{ "GBP", new Currency("GBP", 826, null, "Pound Sterling") },
//			{ "GEL", new Currency("GEL", 981, null, "Lari") },
//			{ "GHC", new Currency("GHC", 276, null, "Ghana, Cedis") },
//			{ "GHS", new Currency("GHS", 936, null, "Cedi") },
//			{ "GIP", new Currency("GIP", 292, null, "Gibraltar Pound") },
//			{ "GMD", new Currency("GMD", 270, null, "Dalasi") },
//			{ "GNF", new Currency("GNF", 324, null, "Guinea Franc") },
//			{ "GTQ", new Currency("GTQ", 320, null, "Quetzal") },
//			{ "GWP", new Currency("GWP", 624, null, "Guinea-Bissau Peso") },
//			{ "GYD", new Currency("GYD", 328, null, "Guyana Dollar") },
//			{ "HKD", new Currency("HKD", 344, null, "Hong Kong Dollar") },
//			{ "HNL", new Currency("HNL", 340, null, "Lempira") },
//			{ "HRK", new Currency("HRK", 191, null, "Croatian Kuna") },
//			{ "HTG", new Currency("HTG", 332, null, "Gourde") },
//			{ "HUF", new Currency("HUF", 348, null, "Forint") },
//			{ "IDR", new Currency("IDR", 360, null, "Rupiah") },
//			{ "ILS", new Currency("ILS", 376, null, "New Israeli Sheqel") },
//			{ "INR", new Currency("INR", 356, null, "Indian Rupee") },
//			{ "IQD", new Currency("IQD", 368, null, "Iraqi Dinar") },
//			{ "IRR", new Currency("IRR", 364, null, "Iranian Rial") },
//			{ "ISK", new Currency("ISK", 352, null, "Iceland Krona") },
//			{ "IUA", new Currency("IUA", -1, null, null) },
//			{ "JMD", new Currency("JMD", 388, null, "Jamaican Dollar") },
//			{ "JOD", new Currency("JOD", 400, null, "Jordanian Dinar") },
//			{ "JPY", new Currency("JPY", 392, null, "Yen") },
//			{ "KES", new Currency("KES", 404, null, "Kenyan Shilling") },
//			{ "KGS", new Currency("KGS", 417, null, "Som") },
//			{ "KHR", new Currency("KHR", 116, null, "Riel") },
//			{ "KMF", new Currency("KMF", 174, null, "Comoro Franc") },
//			{ "KPW", new Currency("KPW", 408, null, "North Korean Won") },
//			{ "KRW", new Currency("KRW", 410, null, "Won") },
//			{ "KWD", new Currency("KWD", 414, null, "Kuwaiti Dinar") },
//			{ "KYD", new Currency("KYD", 136, null, "Cayman Islands Dollar") },
//			{ "KZT", new Currency("KZT", 398, null, "Tenge") },
//			{ "LAK", new Currency("LAK", 418, null, "Kip") },
//			{ "LBP", new Currency("LBP", 422, null, "Lebanese Pound") },
//			{ "LKR", new Currency("LKR", 144, null, "Sri Lanka Rupee") },
//			{ "LRD", new Currency("LRD", 430, null, "Liberian Dollar") },
//			{ "LSL", new Currency("LSL", 426, null, "Loti") },
//			{ "LTL", new Currency("LTL", 440, null, "Lithuanian Litas") },
//			{ "LVL", new Currency("LVL", 428, null, "Latvian Lats") },
//			{ "LYD", new Currency("LYD", 434, null, "Libyan Dinar") },
//			{ "MAD", new Currency("MAD", 504, null, "Moroccan Dirham") },
//			{ "MDL", new Currency("MDL", 498, null, "Moldovan Leu") },
//			{ "MGA", new Currency("MGA", 969, null, "Malagasy Ariary") },
//			{ "MKD", new Currency("MKD", 807, null, "Denar") },
//			{ "MMK", new Currency("MMK", 104, null, "Kyat") },
//			{ "MNT", new Currency("MNT", 496, null, "Tugrik") },
//			{ "MOP", new Currency("MOP", 446, null, "Pataca") },
//			{ "MRO", new Currency("MRO", 478, null, "Ouguiya") },
//			{ "MTL", new Currency("MTL", 46, null, "Malta, Liri") },
//			{ "MUR", new Currency("MUR", 480, null, "Mauritius Rupee") },
//			{ "MVR", new Currency("MVR", 462, null, "Rufiyaa") },
//			{ "MWK", new Currency("MWK", 454, null, "Kwacha") },
//			{ "MXN", new Currency("MXN", 484, null, "Mexican Peso") },
//			{ "MXV", new Currency("MXV", 979, null, "Mexican Unidad de Inversion (UDI)") },
//			{ "MYR", new Currency("MYR", 458, null, "Malaysian Ringgit") },
//			{ "MZM", new Currency("MZM", 366, null, "Mozambique, Meticais") },
//			{ "MZN", new Currency("MZN", 943, null, "Metical") },
//			{ "NAD", new Currency("NAD", 516, null, "Namibia Dollar") },
//			{ "NGN", new Currency("NGN", 566, null, "Naira") },
//			{ "NIO", new Currency("NIO", 558, null, "Cordoba Oro") },
//			{ "NOK", new Currency("NOK", 578, null, "Norwegian Krone") },
//			{ "NPR", new Currency("NPR", 524, null, "Nepalese Rupee") },
//			{ "NUC", new Currency("NUC", 0, null, "Neutral unit of construction") },
//			{ "NZD", new Currency("NZD", 554, null, "New Zealand Dollar") },
//			{ "OMR", new Currency("OMR", 512, null, "Rial Omani") },
//			{ "PAB", new Currency("PAB", 590, null, "Balboa") },
//			{ "PEN", new Currency("PEN", 604, null, "Nuevo Sol") },
//			{ "PGK", new Currency("PGK", 598, null, "Kina") },
//			{ "PHP", new Currency("PHP", 608, null, "Philippine Peso") },
//			{ "PKR", new Currency("PKR", 586, null, "Pakistan Rupee") },
//			{ "PLN", new Currency("PLN", 985, null, "Zloty") },
//			{ "PYG", new Currency("PYG", 600, null, "Guarani") },
//			{ "QAR", new Currency("QAR", 634, null, "Qatari Rial") },
//			{ "ROL", new Currency("ROL", 66, null, "Romania, Lei [being phased out]") },
//			{ "RON", new Currency("RON", 946, null, "New Leu") },
//			{ "RSD", new Currency("RSD", 941, null, "Serbian Dinar") },
//			{ "RUB", new Currency("RUB", 643, "РУБ", "Russian Ruble") },
//			{ "RWF", new Currency("RWF", 646, null, "Rwanda Franc") },
//			{ "SAR", new Currency("SAR", 682, null, "Saudi Riyal") },
//			{ "SBD", new Currency("SBD", 90, null, "Solomon Islands Dollar") },
//			{ "SCR", new Currency("SCR", 690, null, "Seychelles Rupee") },
//			{ "SDG", new Currency("SDG", 938, null, "Sudanese Pound") },
//			{ "SEK", new Currency("SEK", 752, null, "Swedish Krona") },
//			{ "SGD", new Currency("SGD", 702, null, "Singapore Dollar") },
//			{ "SHP", new Currency("SHP", 654, null, "Saint Helena Pound") },
//			{ "SIT", new Currency("SIT", 91, null, "Slovenia, Tolars") },
//			{ "SKK", new Currency("SKK", 63, null, "Slovakia, Koruny") },
//			{ "SLL", new Currency("SLL", 694, null, "Leone") },
//			{ "SOS", new Currency("SOS", 706, null, "Somali Shilling") },
//			{ "SRD", new Currency("SRD", 968, null, "Surinam Dollar") },
//			{ "STD", new Currency("STD", 678, null, "Dobra") },
//			{ "SVC", new Currency("SVC", 222, null, "El Salvador Colon") },
//			{ "SYP", new Currency("SYP", 760, null, "Syrian Pound") },
//			{ "SZL", new Currency("SZL", 748, null, "Lilangeni") },
//			{ "THB", new Currency("THB", 764, null, "Baht") },
//			{ "TJS", new Currency("TJS", 972, null, "Somoni") },
//			{ "TMT", new Currency("TMT", 934, null, "Manat") },
//			{ "TND", new Currency("TND", 788, null, "Tunisian Dinar") },
//			{ "TOP", new Currency("TOP", 776, null, "Pa'anga") },
//			{ "TRY", new Currency("TRY", 949, null, "Turkish Lira") },
//			{ "TTD", new Currency("TTD", 780, null, "Trinidata and Tobago Dollar") },
//			{ "TWD", new Currency("TWD", 901, null, "New Taiwan Dollar") },
//			{ "TZS", new Currency("TZS", 834, null, "Tanzanian Shilling") },
//			{ "UAH", new Currency("UAH", 980, null, "Hryvnia") },
//			{ "UGX", new Currency("UGX", 800, null, "Uganda Shilling") },
//			{ "USD", new Currency("USD", 840, null, "US Dollar") },
//			{ "USN", new Currency("USN", 997, null, "US Dollar (Next Day)") },
//			{ "USS", new Currency("USS", 998, null, "US Dollar (Same Day)") },
//			{ "UYI", new Currency("UYI", 940, null, "Uruguay Peso en Unidades Indexadas") },
//			{ "UYU", new Currency("UYU", 858, null, "Peso Uruguayo") },
//			{ "UZS", new Currency("UZS", 860, null, "Uzbekistan Sum") },
//			{ "VEF", new Currency("VEF", 937, null, "Bolivar Fuerte") },
//			{ "VND", new Currency("VND", 704, null, "Dong") },
//			{ "VUV", new Currency("VUV", 548, null, "Vatu") },
//			{ "WST", new Currency("WST", 882, null, "Tala") },
//			{ "XAF", new Currency("XAF", 950, null, "CFA Franc BEAC") },
//			{ "XAG", new Currency("XAG", 961, null, "Silver") },
//			{ "XAU", new Currency("XAU", 959, null, "Gold") },
//			{ "XBA", new Currency("XBA", 955, null, "Bond Markets Units European Composite Unit (EURCO)") },
//			{ "XBB", new Currency("XBB", 956, null, "European Monetary Unit (E.M.U.-6)") },
//			{ "XBC", new Currency("XBC", 957, null, "European Unit of Account 9 (E.U.A.-9)") },
//			{ "XBD", new Currency("XBD", 958, null, "European Unit of Account 17 (E.U.A.-17)") },
//			{ "XCD", new Currency("XCD", 951, null, "East Caribbean Dollar") },
//			{ "XDR", new Currency("XDR", 960, null, "SDR") },
//			{ "XOF", new Currency("XOF", 952, null, "CFA Franc BCEAO") },
//			{ "XPD", new Currency("XPD", 964, null, "Palladium") },
//			{ "XPF", new Currency("XPF", 953, null, "CFP Franc") },
//			{ "XPT", new Currency("XPT", 962, null, "Platinum") },
//			{ "XTS", new Currency("XTS", 963, null, "Codes specifically reserved for testing purposes") },
//			{ "XXX", new Currency("XXX", 999, null, "The codes assigned for transactions where no currency is involved are:") },
//			{ "YER", new Currency("YER", 886, null, "Yemeni Rial") },
//			{ "ZAR", new Currency("ZAR", 710, null, "Rand") },
//			{ "ZMK", new Currency("ZMK", 894, null, "Zambian Kwacha") },
//			{ "ZWD", new Currency("ZWD", 382, null, "Zimbabwe, Zimbabwe Dollars") },
//			{ "ZWL", new Currency("ZWL", 932, null, "Zimbabwe Dollar") },

//		};

//	}

//}