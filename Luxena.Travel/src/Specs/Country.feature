Feature: Country


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Страны"


Scenario: Create a new Country
	When the user is going to begin to create Country
	And the user fills out the form as follows
		| Label                | Value            |
		| Название             | Новая страна     |
		| Код (2-х сим.)       | NС               |
		| Код (3-х сим.)       | NСT              |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Страны: добавлено Новая страна"


Scenario: Create a new country without required fields
	When the user is going to begin to create Country
	And the user fills out the form as follows
		| Label                | Value            |
		| Название             |                  |
	And the user presses "Сохранить"
	Then the system shows validation message "Название: Это поле обязательно для заполнения"

Scenario: Delete an Country
	When the user selects "DL" in the list
	And the user presses "удалить"
	Then the system shows confirmation dialog

	When the user presses at confirmation dialog "Да"
	Then the system shows notification "Страны: удалено Страна для удаления"