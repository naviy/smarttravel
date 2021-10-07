Feature: Company


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Организации"


Scenario: Create a new Company
	When the user is going to begin to create Company
	And the user fills out the form as follows
		| Label                | Value                 |
		| Наименование         | Новая организация     |
		| Официальное название | New Company           |
		| Фактический адрес    | Фактический адрес     |
		| Юридический адрес    | Юридический адрес     |
		| Код предприятия      | ЕГРПОУ 22             |
		| Телефоны             | 444-44-44             | 
		| E-mail адреса        | test@google.com       | 
		| Факс                 | 555-55-55             | 
		| Веб адрес            | www.test.com.ua       | 
		| Примечание           | Тестовый пользователь |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Организации: добавлено Новая организация"


Scenario: Create a new company without required fields
	When the user is going to begin to create Company
	And the user fills out the form as follows
		| Label             | Value                 |
		| Наименование      |                       |
	And the user presses "Сохранить"
	Then the system shows validation message "Наименование: Это поле обязательно для заполнения"

Scenario: Delete a Company
	When the user selects "DELETE 22" in the list
	And the user presses "удалить"
	Then the system shows confirmation dialog
	When the user presses at confirmation dialog "Да"
	Then the system shows notification "Организации: удалено Организация для удаления"