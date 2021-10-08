module Luxena.Views
{

	registerEntityControllers(sd.AirlineServiceClass, se => [se.Airline, se.Code, se.ServiceClass, ]);
	registerEntityControllers(sd.AccommodationType, se => [ se.Name, se.Description, ]);
	registerEntityControllers(sd.BankAccount, se => [se.Name, se.IsDefault, se.Description, se.Note]);
	registerEntityControllers(sd.CateringType, se => [se.Name, se.Description, ]);
	registerEntityControllers(sd.CurrencyDailyRate, se => [se.Date, se.UAH_EUR, se.UAH_RUB, se.UAH_USD, se.RUB_EUR, se.RUB_USD, se.EUR_USD, ]);
	registerEntityControllers(sd.DocumentAccess, se => [se.Person, se.Owner, se.FullDocumentControl, ]);
	registerEntityControllers(sd.DocumentOwner, se => [se.Owner, se.IsActive, ]);
	registerEntityControllers(sd.GenericProductType, se => [se.Name, ]);
	registerEntityControllers(sd.PaymentSystem, se => [se.Name, ]);

}