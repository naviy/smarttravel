Feature: Airlines Catalog

Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Авиакомпании"



Scenario: Add new airline
	When the user presses "создать"
	Then the system opens form "Авиакомпания (создание)"
	When the user fills out the form as follows
		| Label                            | Value             |
		| Название                         | Авикомпания 1     |
		| IATA код                         | AX                |
		| Prefix код                       | 123               |
		| Требование паспортных данных     | Требуется         |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Авиакомпании: добавлено Авикомпания 1"



Scenario Outline: Cannot create an airline with existing name, IATA code or prefix code
	Given airline "AEROSVIT AIRLINES" with "VV" IATA code and "870" prefix code exists
	When the user tries to create "<name>" airline with "<iata>" IATA code and "<prefix>" prefix code
	Then the system shows error "<error>"

Examples:
	| name              | iata | prefix | error                                            |
	| AEROSVIT AIRLINES | ZZ   | 999    | Авиакомпания с таким названием уже существует    |
	| Airline X         | VV   | 999    | Авиакомпания с таким IATA кодом уже существует   |
	| Airline X         | ZZ   | 870    | Авиакомпания с таким prefix кодом уже существует |
