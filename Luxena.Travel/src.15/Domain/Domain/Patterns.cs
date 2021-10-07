using System;
using Luxena.Domain;


namespace Luxena.Travel.Domain.Patterns
{

	[RU("Место прибытия", ruShort: "место")]
	public class ArrivalPlace : Attribute { }


	[RU("Дата прибытия", ruShort: "дата")]
	public class ArrivalDate : Attribute { }


	[RU("Дата и время прибытия", ruShort: "дата и время")]
	public class ArrivalDateTime : Attribute { }


	[RU("Время прибытия", ruShort: "время")]
	public class ArrivalTime : Attribute { }


	[RU("Ответственный")]
	public class AssignedTo : Attribute { }

	[RU(ruShort: "автоматически")]
	public class Auto : Attribute { }

	[RU("Код")]
	public class Code : Attribute { }


	[RU("Подтверждение пароля"), Luxena.Domain.ConfirmPassword("NewPassword")]
	public class ConfirmPassword : Attribute { }


	[RU("Валюта"), Length(3)]
	public class Currency : Attribute { }


	[RU("Курс валюты"), Float(4)]
	public class CurrencyRate : Attribute { }



	[EN("Date"), RU("Дата"), Luxena.Domain.Date]
	public class Date : Attribute { }


	[RU("С даты"), Luxena.Domain.Date]
	public class DateFrom : Attribute { }


	[RU("По дату"), Luxena.Domain.Date]
	public class DateTo : Attribute { }



	[RU("Место отправления", ruShort: "место")]
	public class DeparturePlace : Attribute { }


	[RU("Дата отправления", ruShort: "дата")]
	public class DepartureDate : Attribute { }


	[RU("Дата и время отправления", ruShort: "дата и время")]
	public class DepartureDateTime : Attribute { }


	[RU("Время отправления", ruShort: "время")]
	public class DepartureTime : Attribute { }


	[RU("Описание")]
	[Text]
	public class Description : Attribute { }


	[RU("Скидка")]
	public class Discount : Attribute { }


	[RU("Дата окончания")]
	public class FinishDate : Attribute { }


	[RU("Дата выпуска")]
	public class IssueDate : Attribute { }


	[RU("Оплачен")]
	public class IsPaid : Attribute { }


	[RU("Аннулирован")]
	public class IsVoid : Attribute { }


	[RU("Месяц"), Length(14)]
	public class Month : Attribute { }


	[RU("Название")]
	public class Name : Attribute { }


	[RU("Новый пароль"), Luxena.Domain.Password]
	public class NewPassword : Attribute { }


	[RU("Примечание")]
	[Text]
	public class Note : Attribute { }


	[RU("Номер"), Length(10)]
	public class Number : Attribute { }


	[RU("Владелец")]
	public class Owner : Attribute { }


	[RU("Пассажир")]
	public class Passenger : Attribute { }


	[RU("Пароль"), Luxena.Domain.Password]
	public class Password : Attribute { }

	
	[RU("№ позиции"), Length(3)]
	public class Position : Attribute { }

	[RU("Плательщик")]
	public class Payer : Attribute { }


	[RU("Цена")]
	public class Price : Attribute { }


	[RU("Количество")]
	public class Quantity : Attribute { }


	[RU("Получатель")]
	public class Recipient : Attribute { }


	[RU("Квартал")]
	public class Quarter : Attribute { }


	[RU("№"), Length(3)]
	public class Rank : Attribute { }


	[RU("Перевыпуск для")]
	public class ReissueFor : Attribute { }


	[RU("Продавец")]
	public class Seller : Attribute { }


	[RU("Сервис-класс")]
	public class ServiceClass : Attribute { }


	[RU("Сервисный сбор")]
	public class ServiceFee : Attribute { }


	[RU("Дата начала")]
	public class StartDate : Attribute { }


	[RU("Статус")]
	public class Status : Attribute { }


	[EN("Time"), RU("Время"), Luxena.Domain.Date]
	public class Time : Attribute { }


	[RU("Итого")]
	public class Total : Attribute { }


	[RU("Тип")]
	public class Type : Attribute { }


	[RU("В т.ч. НДС")]
	public class Vat : Attribute { }


	[RU("Неизвестно")]
	public class Unknown : Attribute { }


	[RU("Год")]
	public class Year : Attribute { }

}