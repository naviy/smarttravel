Feature: Import documents from GDS systems

Scenario: Import an Amadeus document
	Given the user has logged in as Admin
	And the user has selected menu item "Управление->Gds-файлы"
	When the user selects "AIR_20111205162729.20587.PDT" in the list
	And the user presses "импортировать"
	Then the system shows notification "Gds-файлы: Импортировано 1 документ"
