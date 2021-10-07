Feature: Airport


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Аэропорты"


Scenario: Create a new Airport
	When the user is going to begin to create Airport
	And the user fills out the form as follows
		| Label                    | Value             |
		| Код                      | TST               |
		#| Страна                   | USA              |
		| Населенный пункт (англ.) | Los Testos        |
		| Населенный пункт         | Лос Тестос        |
		| Название                 | Новый аэропорт    |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Аэропорты: добавлено TST"


Scenario: Create a new airport without required fields
	When the user is going to begin to create Airport
	And the user fills out the form as follows
		| Label                    | Value                 |
		| Код                      |                       |
	And the user presses "Сохранить"
	Then the system shows validation message "Код: Это поле обязательно для заполнения"


Scenario: Delete an Airport
	When the user selects "DLT" in the list
	And the user presses "удалить"
	Then the system shows confirmation dialog

	When the user presses at confirmation dialog "Да"
	Then the system shows notification "Аэропорты: удалено DLT"