Feature: Currency


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Валюты"

Scenario: Create a new Currency
	When the user is going to begin to create Currency
	And the user fills out the form as follows
		| Label                    | Value             |
		| Код                      | TPD               |
		| Числовой код             | 99999             |
		| Кириллический код        | ТПД               |
		| Название                 | Новая валюта      |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Валюты: добавлено TPD"

Scenario: Create a new сurrency without required fields
	When the user is going to begin to create Currency
	And the user fills out the form as follows
		| Label                    | Value             |
		| Код                      |                   |
	And the user presses "Сохранить"
	Then the system shows validation message "Код: Это поле обязательно для заполнения"

	When the user fills out the form as follows
		| Label                    | Value             |
		| Код                      | TPD               |
		| Числовой код             |                   |
	And the user presses "Сохранить"
	Then the system shows validation message "Числовой код: Это поле обязательно для заполнения"

Scenario: Delete a Currency
	When the user selects "DLT" in the list
	And the user presses "удалить"
	Then the system shows confirmation dialog

	When the user presses at confirmation dialog "Да"
	Then the system shows notification "Валюты: удалено DLT"